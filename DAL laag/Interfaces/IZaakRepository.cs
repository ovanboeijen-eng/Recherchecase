using System;
using System.Collections.Generic;
using System.Text;
using DAL_laag.DTO;

namespace DAL_laag.Interface;

    public interface IZaakRepository
    {
        bool MaakZaakAan(ZaakDTO zaak);
        void WijzigZaak(ZaakDTO zaak);
        ZaakDTO BekijkZaak(int zaakId);
        void VerwijderZaak(int zaakId);
        void VoegBewijsstukToeAanZaak(int zaakId, BewijsstukDTO bewijsstuk);
        
        void VerwijderBewijsstukUitZaak(int zaakId, int bewijsstukId);
        void WijzigBewijsstukInZaak(int zaakId, BewijsstukDTO bewijsstuk);

        void WijzigNotitieInZaak(int zaakId, string notitie);

    }

