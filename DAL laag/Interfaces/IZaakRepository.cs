using System;
using System.Collections.Generic;
using System.Text;
using DAL_laag.DTO;

namespace DAL_laag.Interface;

    public interface IZaakRepository
    {
        bool MaakZaakAan(ZaakDTO zaak);
        bool WijzigZaak(ZaakDTO zaak);
        ZaakDTO? BekijkZaak(int zaakId);
        List<ZaakDTO> BekijkAlleZaken();
        bool VerwijderZaak(int zaakId);
        bool VoegBewijsstukToeAanZaak(int zaakId, BewijsstukDTO bewijsstuk);
        bool WijzigNotitieInZaak(int zaakId, string notitie);

}
        
    

