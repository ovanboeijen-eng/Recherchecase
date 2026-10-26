using System;
using System.Collections.Generic;
using System.Text;
using Logic_layer.Service;

using Recherchecase.Models;
using Microsoft.AspNetCore.Mvc;

namespace Recherchecase.Controllers
{
    public class ZaakController : Controller
    {
        private readonly ZaakService _zaakService;

        public ZaakController(ZaakService zaakService)
        {
            _zaakService = zaakService;
        }

        IActionResult BekijkZaak(int zaakId)
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




    }
}
