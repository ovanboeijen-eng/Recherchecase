using System;
using System.Collections.Generic;
using System.Text;
using Logic_layer.Classes;

namespace Logic_layer.Service
{
    public class ZaakService
    {
        
        public void MaakZaakAan(ZaakModel zaak)
        {
            if (string.IsNullOrEmpty(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }
            // _zaakRepository.MaakZaakAan(zaak);

        }

        public void WijzigZaak(ZaakModel zaak)
        {
            if (string.IsNullOrEmpty(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }
            // _zaakRepository.WijzigZaak(zaak);
        }

        public int BekijkZaak(int zaakId)
        {
            // _zaakRepository.BekijkZaak(zaakId);
            return 0;
        }

        public int VerwijderZaak(int zaakId)
        {
            //  _zaakRepository.VerwijderZaak(zaakId);
            return 0;
        }

        public void VoegBewijsstukToeAanZaak(int zaakId, BewijsstukModel bewijsstuk)
        {
            if (string.IsNullOrEmpty(bewijsstuk.Titel))
            {
                throw new ArgumentException("Titel van bewijsstuk mag niet leeg zijn.");
            }
            // _zaakRepository.VoegBewijsstukToeAanZaak(zaakId, bewijsstuk);
        }

        public void VerwijderBewijsstukVanZaak(int zaakId, int bewijsstukId)
        {
            // _zaakRepository.VerwijderBewijsstukVanZaak(zaakId, bewijsstukId);
        }

        public void WijzigNotitieVanZaak(int zaakId, string notitie)
        {
            // _zaakRepository.WijzigNotitieVanZaak(zaakId, notitie);
        }

    }
}
