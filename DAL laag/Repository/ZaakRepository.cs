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

        public ZaakDTO? BekijkZaak(int zaakId)
        {
            if (zaakId <= 0) throw new ArgumentException("Invalid zaakId", nameof(zaakId));

            using var connection = _database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Titel, Beschrijving, Datum, Notitie FROM Zaak WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", zaakId);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            else
            { 
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

        }

        public bool MaakZaakAan(ZaakDTO zaak)
        {
            if (zaak == null) throw new ArgumentNullException(nameof(zaak));
            using var connection = _database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Zaak (Titel, Beschrijving, Datum, Notitie) VALUES (@Titel, @Beschrijving, @Datum, @Notitie); SELECT CAST(SCOPE_IDENTITY() AS int);";
            command.Parameters.AddWithValue("@Titel", zaak.Titel);
            command.Parameters.AddWithValue("@Beschrijving", (object)zaak.Beschrijving ?? DBNull.Value);
            command.Parameters.AddWithValue("@Datum", zaak.Datum == default ? DateTime.Now : zaak.Datum);
            command.Parameters.AddWithValue("@Notitie", (object)zaak.Notitie ?? DBNull.Value);

            var insertedId = command.ExecuteScalar();
            if (insertedId is not int id || id <= 0) return false;
            zaak.Id = id;
            return true;
        }

        public bool VerwijderBewijsstukUitZaak(int zaakId, int bewijsstukId)
        {
            throw new NotImplementedException();
        }

        public bool VerwijderZaak(int zaakId)
        {
            throw new NotImplementedException();
        }

        public bool VoegBewijsstukToeAanZaak(int zaakId, BewijsstukDTO bewijsstuk)
        {
            throw new NotImplementedException();
        }

        public bool WijzigBewijsstukInZaak(int zaakId, BewijsstukDTO bewijsstuk)
        {
            throw new NotImplementedException();
        }

        public bool WijzigNotitieInZaak(int zaakId, string notitie)
        {
            throw new NotImplementedException();
        }

        public bool WijzigZaak(ZaakDTO zaak)
        {
            if (zaak == null) throw new ArgumentNullException(nameof(zaak));
            using var connection = _database.GetConnection();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE Zaak SET Titel = @Titel, Notitie = @Notitie WHERE Id = @Id";
            command.Parameters.AddWithValue("@Titel", zaak.Titel);
            command.Parameters.AddWithValue("@Notitie", (object?)zaak.Notitie ?? DBNull.Value);
            command.Parameters.AddWithValue("@Id", zaak.Id);
            return command.ExecuteNonQuery() > 0;
        }

        bool IZaakRepository.WijzigNotitieInZaak(int zaakId, string notitie)
        {
            throw new NotImplementedException();
        }

        bool IZaakRepository.WijzigZaak(ZaakDTO zaak)
        {
            return WijzigZaak(zaak);
        }
    }
}
