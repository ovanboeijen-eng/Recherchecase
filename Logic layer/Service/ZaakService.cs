using Logic_layer.Models;
using DAL_laag.Interface;
using DAL_laag.DTO;
using System;

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
            zaak.AangemaaktOp = DateTime.Now; // Set the creation date to the current date and time
            var dto = MapZaakToDTO(zaak);
            var gelukt = _zaakRepository.MaakZaakAan(dto);
            if (gelukt)
            {
                zaak.Id = dto.Id;
                zaak.Nummer = dto.Nummer;
            }
            return gelukt;
        }

        public ZaakModel BekijkZaak(int zaakId)
        {
            var zaak = _zaakRepository.BekijkZaak(zaakId);
            if (zaak == null)
            {
                throw new ArgumentException("Zaak niet gevonden.");
            }
            return MapZaakToModel(zaak);
        }

        public bool VerwijderZaak(int zaakId)
        {
            if (zaakId <= 0) throw new ArgumentOutOfRangeException(nameof(zaakId));
            return _zaakRepository.VerwijderZaak(zaakId);
        }

        public bool WijzigZaak(ZaakModel zaak)
        {
            if (zaak == null) throw new ArgumentNullException(nameof(zaak));
            if (string.IsNullOrWhiteSpace(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.", nameof(zaak.Titel));
            }
            return _zaakRepository.WijzigZaak(MapZaakToDTO(zaak));
        }

    

   
        public bool WijzigNotitieVanZaak(int zaakId, string notitie)
        {
            if (zaakId <= 0) throw new ArgumentOutOfRangeException(nameof(zaakId));
            return _zaakRepository.WijzigNotitieInZaak(zaakId, notitie);
        }

        private static ZaakDTO MapZaakToDTO(ZaakModel zaak) =>
            new ZaakDTO
            {
                Id = zaak.Id,
                Nummer = zaak.Nummer,
                Titel = zaak.Titel,
                Notitie = zaak.Notitie,
                AangemaaktOp = zaak.AangemaaktOp
            };

        private ZaakModel MapZaakToModel(ZaakDTO zaak)
        {
            return new ZaakModel(zaak.Id, zaak.Titel, zaak.Notitie ?? string.Empty)
            {
                Nummer = zaak.Nummer,
                AangemaaktOp = zaak.AangemaaktOp
            };
      
        }
    }
}
