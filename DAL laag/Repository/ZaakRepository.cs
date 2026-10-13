using DataAccess.Database;
using DAL_laag.Interface;
using DAL_laag.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;


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
            if (zaakId <= 0) throw new ArgumentException("Invalid zaakId", nameof(zaakId));

            using var connection = _database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Titel, Beschrijving, Datum, Notitie FROM Zaak WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", zaakId);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            var dto = new ZaakDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Titel = reader.GetString(reader.GetOrdinal("Titel")),
                Beschrijving = reader.IsDBNull(reader.GetOrdinal("Beschrijving")) ? null : reader.GetString(reader.GetOrdinal("Beschrijving")),
                Datum = reader.GetDateTime(reader.GetOrdinal("Datum")),
                Notitie = reader.IsDBNull(reader.GetOrdinal("Notitie")) ? null : reader.GetString(reader.GetOrdinal("Notitie"))
            };

            return dto;
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
