using Logic_layer.Models;
using Logic_layer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic_layer.Service
{
    public class ZaakService
    {
        private readonly IZaakRepository _zaakRepository;

        public ZaakService(IZaakRepository repository)
        {
            _zaakRepository = repository;
        }


        public bool MaakZaakAan(ZaakModel zaak)
        {
            if (zaak == null) throw new ArgumentNullException(nameof(zaak));
            if (string.IsNullOrWhiteSpace(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.", nameof(zaak.Titel));
            }

            try
            {
                _zaakRepository.MaakZaakAan(zaak);
                return true;
            }
            catch (Exception)
            {
              
                return false;
            }
        }

        public void WijzigZaak(ZaakModel zaak)
        {
            if (string.IsNullOrEmpty(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }
             _zaakRepository.WijzigZaak(zaak);
        }

        public int BekijkZaak(int zaakId)
        {
             _zaakRepository.BekijkZaak(zaakId);
            return 0;
        }

        public int VerwijderZaak(int zaakId)
        {
              _zaakRepository.VerwijderZaak(zaakId);
            return 0;
        }

        public void VoegBewijsstukToeAanZaak(int zaakId, BewijsstukModel bewijsstuk)
        {
            if (string.IsNullOrEmpty(bewijsstuk.Titel))
            {
                throw new ArgumentException("Titel van bewijsstuk mag niet leeg zijn.");
            }
             _zaakRepository.VoegBewijsstukToeAanZaak(zaakId, bewijsstuk);
        }

        public void VerwijderBewijsstukVanZaak(int zaakId, int bewijsstukId)
        {
             _zaakRepository.VerwijderBewijsstukUitZaak(zaakId, bewijsstukId);
        }

        public void WijzigBewijsstukInZaak(int zaakId, BewijsstukModel bewijsstuk)
        {
            if (string.IsNullOrEmpty(bewijsstuk.Titel))
            {
                throw new ArgumentException("Titel van bewijsstuk mag niet leeg zijn.");
            }
            if (string.IsNullOrEmpty(bewijsstuk.Soort))
            {
                throw new ArgumentException("Soort van bewijsstuk mag niet leeg zijn.");
            }
            _zaakRepository.WijzigBewijsstukInZaak(zaakId, bewijsstuk);
        }
        public void WijzigNotitieVanZaak(int zaakId, string notitie)
        {
             _zaakRepository.WijzigNotitieInZaak(zaakId, notitie);
        }

    }
}
