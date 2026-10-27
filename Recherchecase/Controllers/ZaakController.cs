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

        public ZaakController(ZaakService zaakService)
        {
            _zaakService = zaakService;
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
            if (zaakId <= 0) return NotFound();
            ZaakModel zaak;
            try { zaak = _zaakService.BekijkZaak(zaakId); }
            catch (ArgumentException) { return NotFound(); }
            if (zaak == null)
            {
                return NotFound();
            }
            ZaakViewModel viewModel = new ZaakViewModel
            {
                Id = zaakId,
                Titel = zaak.Titel,
                Notitie = zaak.Notitie
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult MaakZaakAan(ZaakViewModel zaak)
        {
            if (!ModelState.IsValid)
            {
                return View(zaak);
            }

            var model = new ZaakModel(0, zaak.Titel, zaak.Notitie);
            bool gelukt = _zaakService.MaakZaakAan(model);

            if (!gelukt)
            {
                ModelState.AddModelError("", "De zaak kon niet worden aangemaakt.");
                return View(zaak);
            }

            TempData["Success"] = "De zaak is aangemaakt.";
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
            bool gelukt = _zaakService.WijzigZaak(new ZaakModel(viewModel.Id, viewModel.Titel, viewModel.Notitie));
            if (!gelukt)
            {
                ModelState.AddModelError("", "De zaak kon niet worden gewijzigd.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(BekijkZaak), new { zaakId = viewModel.Id });
        }

    }
}
