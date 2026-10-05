using System;
using System.Collections.Generic;
using System.Text;
using Logic_layer.Classes;

namespace Logic_layer.Service
{
    public class ZaakService
    {
        //   private readonly ZaakRepository _zaakRepository;

        //  public ZaakService(ZaakRepository zaakRepository)
        //   {
        //       _zaakRepository = zaakRepository;
        //    } 

        public void MaakZaakAan(Zaak zaak)
        {
            if (string.IsNullOrEmpty(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }
            // _zaakRepository.MaakZaakAan(zaak);

        }

        public void WijzigZaak(Zaak zaak)
        {
            if (string.IsNullOrEmpty(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }
            // _zaakRepository.WijzigZaak(zaak);
        }

        public void VerwijderZaak(int zaakId)
        {
            //  _zaakRepository.VerwijderZaak(zaakId);
        }

        public void VoegBewijsstukToeAanZaak(int zaakId, Bewijsstuk bewijsstuk)
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
