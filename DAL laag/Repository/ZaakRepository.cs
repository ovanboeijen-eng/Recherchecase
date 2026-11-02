using DataAccess.Database;
using DAL_laag.DTO;
using DAL_laag.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

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
            command.CommandText = @"
                SELECT TOP (1) Id, Titel, Notities, Nummer, Beschrijving, AangemaaktOp
                FROM dbo.Zaak
                WHERE Id = @Id OR Nummer = @Id
                ORDER BY CASE WHEN Nummer = @Id THEN 0 ELSE 1 END";
            command.Parameters.Add("@Id", SqlDbType.Int).Value = zaakId;

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            return new ZaakDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Titel = reader.IsDBNull(reader.GetOrdinal("Titel")) ? string.Empty : reader.GetString(reader.GetOrdinal("Titel")),
                Notitie = reader.IsDBNull(reader.GetOrdinal("Notities")) ? null : reader.GetString(reader.GetOrdinal("Notities")),
                Nummer = reader.IsDBNull(reader.GetOrdinal("Nummer")) ? reader.GetInt32(reader.GetOrdinal("Id")) : reader.GetInt32(reader.GetOrdinal("Nummer")),
                Beschrijving = reader.IsDBNull(reader.GetOrdinal("Beschrijving")) ? null : reader.GetString(reader.GetOrdinal("Beschrijving")),
                AangemaaktOp = reader.GetDateTime(reader.GetOrdinal("AangemaaktOp"))
            };
        }

        public bool MaakZaakAan(ZaakDTO zaak)
        {
            ArgumentNullException.ThrowIfNull(zaak);
            if (string.IsNullOrWhiteSpace(zaak.Titel))
                throw new ArgumentException("Een zaak moet een titel hebben.", nameof(zaak));

            using var connection = _database.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            int nextId;
            int nextNummer;
            using (var idCommand = connection.CreateCommand())
            {
                idCommand.Transaction = transaction;
                idCommand.CommandText = @"
                    SELECT ISNULL(MAX(Id), 0) + 1, ISNULL(MAX(Nummer), 0) + 1
                    FROM dbo.Zaak WITH (TABLOCKX, HOLDLOCK)";
                using var reader = idCommand.ExecuteReader();
                reader.Read();
                nextId = reader.GetInt32(0);
                nextNummer = reader.GetInt32(1);
            }

            using (var insertCommand = connection.CreateCommand())
            {
                insertCommand.Transaction = transaction;
                insertCommand.CommandText = @"
                    INSERT INTO dbo.Zaak (Id, Titel, Notities, Nummer, Beschrijving, AangemaaktOp)
                    VALUES (@Id, @Titel, @Notities, @Nummer, @Beschrijving, @AangemaaktOp)";
                insertCommand.Parameters.Add("@Id", SqlDbType.Int).Value = nextId;
                insertCommand.Parameters.Add("@Titel", SqlDbType.NVarChar, 50).Value = zaak.Titel.Trim();
                insertCommand.Parameters.Add("@Notities", SqlDbType.NVarChar, -1).Value = (object?)zaak.Notitie ?? DBNull.Value;
                insertCommand.Parameters.Add("@Nummer", SqlDbType.Int).Value = nextNummer;
                insertCommand.Parameters.Add("@Beschrijving", SqlDbType.NVarChar, -1).Value = (object?)zaak.Beschrijving ?? DBNull.Value;
                insertCommand.Parameters.Add("@AangemaaktOp", SqlDbType.DateTime2).Value = zaak.AangemaaktOp;
                insertCommand.ExecuteNonQuery();
            }

            transaction.Commit();
            zaak.Id = nextId;
            zaak.Nummer = nextNummer;
            return true;
        }

        public bool WijzigZaak(ZaakDTO zaak)
        {
            ArgumentNullException.ThrowIfNull(zaak);
            using var connection = _database.GetConnection();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE dbo.Zaak SET Titel = @Titel, Notities = @Notities WHERE Id = @Id";
            command.Parameters.Add("@Titel", SqlDbType.NVarChar, 50).Value = zaak.Titel.Trim();
            command.Parameters.Add("@Notities", SqlDbType.NVarChar, -1).Value = (object?)zaak.Notitie ?? DBNull.Value;
            command.Parameters.Add("@Id", SqlDbType.Int).Value = zaak.Id;
            return command.ExecuteNonQuery() > 0;
        }

        public bool VerwijderZaak(int zaakId)
        {
            if (zaakId <= 0) throw new ArgumentOutOfRangeException(nameof(zaakId));

            using var connection = _database.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            int? id;
            using (var findCommand = connection.CreateCommand())
            {
                findCommand.Transaction = transaction;
                findCommand.CommandText = @"
                    SELECT TOP (1) Id
                    FROM dbo.Zaak WITH (UPDLOCK, HOLDLOCK)
                    WHERE Id = @ZaakId OR Nummer = @ZaakId
                    ORDER BY CASE WHEN Nummer = @ZaakId THEN 0 ELSE 1 END";
                findCommand.Parameters.Add("@ZaakId", SqlDbType.Int).Value = zaakId;
                var result = findCommand.ExecuteScalar();
                id = result == null || result == DBNull.Value ? null : Convert.ToInt32(result);
            }

            if (id == null)
            {
                transaction.Rollback();
                return false;
            }

            using (var assignmentCommand = connection.CreateCommand())
            {
                assignmentCommand.Transaction = transaction;
                assignmentCommand.CommandText = "DELETE FROM dbo.Rechercheur_Zaak WHERE ZaakId = @ZaakId";
                assignmentCommand.Parameters.Add("@ZaakId", SqlDbType.Int).Value = id.Value;
                assignmentCommand.ExecuteNonQuery();
            }

            using (var deleteCommand = connection.CreateCommand())
            {
                deleteCommand.Transaction = transaction;
                deleteCommand.CommandText = "DELETE FROM dbo.Zaak WHERE Id = @ZaakId";
                deleteCommand.Parameters.Add("@ZaakId", SqlDbType.Int).Value = id.Value;
                if (deleteCommand.ExecuteNonQuery() == 0)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            transaction.Commit();
            return true;
        }
        public bool VoegBewijsstukToeAanZaak(int zaakId, BewijsstukDTO bewijsstuk) => throw new NotImplementedException();
      
        public bool WijzigNotitieInZaak(int zaakId, string notitie)
        {
            if (zaakId <= 0) throw new ArgumentOutOfRangeException(nameof(zaakId));

            using var connection = _database.GetConnection();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE dbo.Zaak SET Notities = @Notities WHERE Id = @Id";
            command.Parameters.Add("@Notities", SqlDbType.NVarChar, -1).Value = (object?)notitie ?? DBNull.Value;
            command.Parameters.Add("@Id", SqlDbType.Int).Value = zaakId;
            return command.ExecuteNonQuery() > 0;
        }

        bool IZaakRepository.WijzigNotitieInZaak(int zaakId, string notitie) => WijzigNotitieInZaak(zaakId, notitie);
        bool IZaakRepository.WijzigZaak(ZaakDTO zaak) => WijzigZaak(zaak);
    }
}
