using Logic_layer.Models;
using DAL_laag.Interface;
using DAL_laag.DTO;
using System;
using System.Security.Cryptography.X509Certificates;

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
            zaak.AangemaaktOp = DateTime.Now;
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

        public List<ZaakModel> BekijkAlleZaken()
        {
            return _zaakRepository.BekijkAlleZaken()
                .Select(MapZaakToModel)
                .ToList();
        }

        public List<BewijsstukModel> BekijkBewijsstukkenBijZaak(int zaakId)
        {
            if (zaakId <= 0) throw new ArgumentOutOfRangeException(nameof(zaakId));

            return _zaakRepository.BekijkBewijsstukkenBijZaak(zaakId)
                .Select(bewijsstuk => new BewijsstukModel(bewijsstuk.Id, bewijsstuk.Titel, bewijsstuk.Soort)
                {
                    Datum = bewijsstuk.Datum
                })
                .ToList();
        }

        public bool VoegBewijsstukToeAanZaak(int zaakId, BewijsstukModel bewijsstuk)
        {
            ArgumentNullException.ThrowIfNull(bewijsstuk);
            if (zaakId <= 0) throw new ArgumentOutOfRangeException(nameof(zaakId));
            if (string.IsNullOrWhiteSpace(bewijsstuk.Titel)) throw new ArgumentException("Vul een titel in.", nameof(bewijsstuk.Titel));
            if (string.IsNullOrWhiteSpace(bewijsstuk.Soort)) throw new ArgumentException("Vul een soort in.", nameof(bewijsstuk.Soort));
            if (bewijsstuk.Datum == null) throw new ArgumentException("Vul een datum in.", nameof(bewijsstuk.Datum));

            var dto = new BewijsstukDTO
            {
                Id = bewijsstuk.Id,
                Titel = bewijsstuk.Titel.Trim(),
                Soort = bewijsstuk.Soort.Trim(),
                Datum = bewijsstuk.Datum
            };

            var gelukt = _zaakRepository.VoegBewijsstukToeAanZaak(zaakId, dto);
            if (gelukt) bewijsstuk.Id = dto.Id;
            return gelukt;
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


        public List<ZaakModel> ZoekZaken(string zoekterm)
        {
            if (string.IsNullOrWhiteSpace(zoekterm))
            {
                throw new ArgumentException("Zoekterm mag niet leeg zijn.", nameof(zoekterm));
            }
            var zaken = _zaakRepository.BekijkAlleZaken();
            return zaken.Select(MapZaakToModel).ToList();
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
