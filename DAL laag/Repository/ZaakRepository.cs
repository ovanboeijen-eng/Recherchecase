using DataAccess.Database;
using DAL_laag.Interfaces;
using DAL_laag.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Logic_layer.Interface;

namespace DAL_laag.Repository
{
    public class ZaakRepository : IZaakRepository
    {
        private readonly DatabaseConnection _database;

        public ZaakRepository(DatabaseConnection database)
        {
            _database = database;
        }

        public ZaakDTO BekijkZaak(int zaakId)
        {
            throw new NotImplementedException();
        }

        public bool MaakZaakAan(ZaakDTO zaak)
        {
            if (zaak == null) throw new ArgumentNullException(nameof(zaak));
            // TODO: add database insert logic
            return false;
        }

        public void VerwijderBewijsstukUitZaak(int zaakId, int bewijsstukId)
        {
            throw new NotImplementedException();
        }

        public void VerwijderZaak(int zaakId)
        {
            throw new NotImplementedException();
        }

        public void VoegBewijsstukToeAanZaak(int zaakId, BewijsstukDTO bewijsstuk)
        {
            throw new NotImplementedException();
        }

        public void WijzigBewijsstukInZaak(int zaakId, BewijsstukDTO bewijsstuk)
        {
            throw new NotImplementedException();
        }

        public void WijzigNotitieInZaak(int zaakId, string notitie)
        {
            throw new NotImplementedException();
        }

        public void WijzigZaak(ZaakDTO  zaak)
        {
            throw new NotImplementedException();
        }
    }
}
