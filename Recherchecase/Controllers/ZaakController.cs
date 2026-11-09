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
                if (zaakId.Value <= 0)
                {
                    ViewBag.SearchError = "Vul een geldig zaaknummer in.";
                }
                else
                {
                    return RedirectToAction(nameof(BekijkZaak), new { zaakId = zaakId.Value });
                }
            }

            try
            {
                var zaken = _zaakService.BekijkAlleZaken()
                    .Select(zaak => new ZaakViewModel
                    {
                        Id = zaak.Id,
                        Nummer = zaak.Nummer,
                        Titel = zaak.Titel,
                        Notitie = zaak.Notitie,
                        AangemaaktOp = zaak.AangemaaktOp
                    })
                    .ToList();

                return View(zaken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ophalen van het zakenoverzicht is mislukt.");
                ViewBag.ListError = "Het zakenoverzicht kon niet worden geladen. Probeer het later opnieuw.";
                return View(new List<ZaakViewModel>());
            }
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

                        ZaakDetailsViewModel viewModel = MaakZaakDetailsViewModel(zaak);
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
        [ValidateAntiForgeryToken]
        public IActionResult VoegBewijsstukToe(int zaakId, ZaakDetailsViewModel viewModel)
        {
            ZaakModel zaak;
            try
            {
                zaak = _zaakService.BekijkZaak(zaakId);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("Bekijkzaak", MaakZaakDetailsViewModel(zaak, viewModel.NieuwBewijsstuk));
            }

            try
            {
                var bewijsstuk = new BewijsstukModel(0, viewModel.NieuwBewijsstuk.Titel, viewModel.NieuwBewijsstuk.Soort)
                {
                    Datum = viewModel.NieuwBewijsstuk.Datum
                };

                if (!_zaakService.VoegBewijsstukToeAanZaak(zaakId, bewijsstuk))
                {
                    ModelState.AddModelError(string.Empty, "Het bewijsstuk kon niet worden opgeslagen.");
                    return View("Bekijkzaak", MaakZaakDetailsViewModel(zaak, viewModel.NieuwBewijsstuk));
                }

                TempData["Success"] = "Het bewijsstuk is aan de zaak toegevoegd.";
                return RedirectToAction(nameof(BekijkZaak), new { zaakId });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Toevoegen van een bewijsstuk aan zaak {ZaakId} is mislukt.", zaakId);
                ModelState.AddModelError(string.Empty, "Opslaan is mislukt. Controleer of de databasekolommen voor bewijsstukken zijn toegevoegd.");
                return View("Bekijkzaak", MaakZaakDetailsViewModel(zaak, viewModel.NieuwBewijsstuk));
            }
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
                Notitie = zaak.Notitie,
                AangemaaktOp = zaak.AangemaaktOp
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
            bool gelukt = _zaakService.WijzigZaak(new ZaakModel(viewModel.Id, viewModel.Titel, viewModel.Notitie)
            {
                Nummer = viewModel.Nummer,
                AangemaaktOp = viewModel.AangemaaktOp
            });
            if (!gelukt)
            {
                ModelState.AddModelError("", "De zaak kon niet worden gewijzigd.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(BekijkZaak), new { zaakId = viewModel.Id });
        }

        private ZaakDetailsViewModel MaakZaakDetailsViewModel(
            ZaakModel zaak,
            VoegBewijsstukToeViewModel? nieuwBewijsstuk = null)
        {
            return new ZaakDetailsViewModel
            {
                Id = zaak.Id,
                Nummer = zaak.Nummer,
                Titel = zaak.Titel,
                Notitie = zaak.Notitie,
                AangemaaktOp = zaak.AangemaaktOp,
                Bewijsstukken = _zaakService.BekijkBewijsstukkenBijZaak(zaak.Id)
                    .Select(bewijsstuk => new BewijsstukViewModel
                    {
                        Id = bewijsstuk.Id,
                        Titel = bewijsstuk.Titel,
                        Soort = bewijsstuk.Soort,
                        Datum = bewijsstuk.Datum
                    })
                    .ToList(),
                NieuwBewijsstuk = nieuwBewijsstuk ?? new VoegBewijsstukToeViewModel()
            };
        }

    }
}
