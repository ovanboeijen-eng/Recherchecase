using Logic_layer.Models;
using Logic_layer.Service;
using Microsoft.AspNetCore.Mvc;
using Recherchecase.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

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
        public IActionResult BekijkZaak(int zaakId)
        {
            var zaak = _zaakService.BekijkZaak(zaakId);
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
        public IActionResult MaakZaakAan(ZaakModel zaak)
        {
            if (!ModelState.IsValid)
            {
                return View(zaak);
            }

            bool gelukt = _zaakService.MaakZaakAan(zaak);

            if (!gelukt)
            {
                ModelState.AddModelError("", "De zaak kon niet worden aangemaakt.");
                return View(zaak);
            }

            return RedirectToAction(nameof(BekijkZaak), new { id = zaak.Id });
        }

        [HttpGet]
        public IActionResult WijzigZaak(int zaakId)
        {
            var zaak = _zaakService.BekijkZaak(zaakId);

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
        public IActionResult WijzigZaak(ZaakModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            bool gelukt = _zaakService.WijzigZaak(viewModel);
            if (!gelukt)
            {
                ModelState.AddModelError("", "De zaak kon niet worden gewijzigd.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(BekijkZaak), new { zaakId = viewModel.Id });
        }

    }
}
