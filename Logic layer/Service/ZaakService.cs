using Logic_layer.Models;
using DAL_laag.Interface;
using DAL_laag.DTO;
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
                _zaakRepository.MaakZaakAan(MapZaakToDTO(zaak));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        public int VerwijderZaak(int zaakId)
        {
            _zaakRepository.VerwijderZaak(zaakId);
            return 0;
        }

        public bool WijzigZaak(ZaakModel zaak)
        {
            if (zaak == null) throw new ArgumentNullException(nameof(zaak));
            if (string.IsNullOrWhiteSpace(zaak.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.", nameof(zaak.Titel));
            }
            _zaakRepository.WijzigZaak(MapZaakToDTO(zaak));
            return true;
        }

        public bool VerwijderBewijsstukVanZaak(int zaakId, int bewijsstukId)
        {
            _zaakRepository.VerwijderBewijsstukUitZaak(zaakId, bewijsstukId);
            return true;
        }
        

        public bool WijzigBewijsstukInZaak(int zaakId, BewijsstukDTO bewijsstuk)
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
            return true;
        }
        public bool WijzigNotitieVanZaak(int zaakId, string notitie)
        {
            _zaakRepository.WijzigNotitieInZaak(zaakId, notitie);
            return true;
        }

        // helper mapper
        private static ZaakDTO MapZaakToDTO(ZaakModel zaak) =>
            new ZaakDTO
            {
                Id = zaak.Id,
                Titel = zaak.Titel,
                Notitie = zaak.Notitie
            };

        private static BewijsstukDTO MapBewijsstukToDTO(BewijsstukModel bewijsstuk) =>
            new BewijsstukDTO
            {
                Id = bewijsstuk.Id,
                Titel = bewijsstuk.Titel,
                Soort = bewijsstuk.Soort
            };

        private ZaakModel MapZaakToModel(ZaakDTO zaak)
        {
            return new ZaakModel(zaak.Id, zaak.Titel, zaak.Notitie);
      
        }
    }
}
