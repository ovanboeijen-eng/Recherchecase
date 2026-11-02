using Logic_layer.Models;
using Logic_layer.Service;
using Microsoft.AspNetCore.Mvc;
using Recherchecase.ViewModels;
using System;

namespace Recherchecase.Controllers
{
    public class ZaakController : Controller
    {
        private readonly ZaakService _zaakService;
        private readonly ILogger<ZaakController> _logger;

        public ZaakController(ZaakService zaakService, ILogger<ZaakController> logger)
        {
            _zaakService = zaakService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int? zaakId)
        {
            if (zaakId.HasValue)
            {
                if (zaakId.Value <= 0) { ViewBag.SearchError = "Vul een geldig zaaknummer in."; return View(); }
                return RedirectToAction(nameof(BekijkZaak), new { zaakId = zaakId.Value });
            }
            return View();
        }

        [HttpGet]
        public IActionResult MaakZaakAan()
        {
            return View(new ZaakViewModel());
        }

        [HttpGet]
        public IActionResult BekijkZaak(int zaakId)
        {
            if(zaakId>0)
            {
                ZaakModel zaak;
                try
                {
                    zaak = _zaakService.BekijkZaak(zaakId);
                    if (zaak != null)
                    {

                        ZaakViewModel viewModel = new ZaakViewModel
                        {
                            Id = zaakId,
                            Nummer = zaak.Nummer,
                            Titel = zaak.Titel,
                            Notitie = zaak.Notitie
                        };
                        return View(viewModel);
                    }
                }
                catch (ArgumentException)
                {
                    return NotFound();
                }

            }
            return NotFound();


        }

        [HttpPost]
        public IActionResult MaakZaakAan(ZaakViewModel zaak)
        {
            if (!ModelState.IsValid)
            {
                return View(zaak);
            }

            var model = new ZaakModel(0, zaak.Titel, zaak.Notitie);
            bool gelukt;
            try
            {
                gelukt = _zaakService.MaakZaakAan(model);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Aanmaken van een zaak is mislukt.");
                ModelState.AddModelError("", "Opslaan is mislukt. Controleer of de database bereikbaar is en probeer het opnieuw. De technische fout staat in de applicatielogboeken.");
                return View(zaak);
            }

            if (!gelukt)
            {
                ModelState.AddModelError("", "De database heeft de zaak niet opgeslagen. Probeer het opnieuw of controleer de applicatielogboeken.");
                return View(zaak);
            }

            TempData["Success"] = $"De zaak is aangemaakt. Zaaknummer: #{model.Nummer}.";
            return RedirectToAction(nameof(BekijkZaak), new { zaakId = model.Id });
        }

        [HttpGet]
        public IActionResult WijzigZaak(int zaakId)
        {
            ZaakModel zaak;
            try { zaak = _zaakService.BekijkZaak(zaakId); }
            catch (ArgumentException) { return NotFound(); }

            if (zaak == null)
            {
                return NotFound();
            }

            ZaakViewModel viewModel = new ZaakViewModel
            {
                Id = zaak.Id,
                Nummer = zaak.Nummer,
                Titel = zaak.Titel,
                Notitie = zaak.Notitie
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WijzigZaak(ZaakViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            bool gelukt = _zaakService.WijzigZaak(new ZaakModel(viewModel.Id, viewModel.Titel, viewModel.Notitie) { Nummer = viewModel.Nummer });
            if (!gelukt)
            {
                ModelState.AddModelError("", "De zaak kon niet worden gewijzigd.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(BekijkZaak), new { zaakId = viewModel.Id });
        }

    }
}
