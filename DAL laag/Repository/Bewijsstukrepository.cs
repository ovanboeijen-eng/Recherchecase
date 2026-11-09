using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DAL_laag.DTO;
using DAL_laag.Interfaces;
using DataAccess.Database;

namespace DAL_laag.Repository
{
    public class BewijsstukRepository : IBewijsstukRepository
    {
        private readonly DatabaseConnection _database;

        public BewijsstukRepository(DatabaseConnection database)
        {
            _database = database;
        }

        public BewijsstukDTO? BekijkBewijsstuk(int bewijsstukId)
        {
            if (bewijsstukId <= 0)
            {
                throw new ArgumentException("Ongeldig bewijsstuk ID.", nameof(bewijsstukId));
            }
            using (var connection = _database.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Titel, Soort, Datum FROM dbo.Bewijsstuk WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", bewijsstukId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BewijsstukDTO
                            {
                                Id = reader.GetInt32(0),
                                Titel = reader.GetString(1),
                                Soort = reader.GetString(2),
                                Datum = reader.IsDBNull(3) ? null : reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }
            return null;
        }
        public bool MaakBewijsstukAan(BewijsstukDTO bewijsstuk)
        {
            ArgumentNullException.ThrowIfNull(bewijsstuk, nameof(bewijsstuk));
            if (string.IsNullOrWhiteSpace(bewijsstuk.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.", nameof(bewijsstuk.Titel));
            }
            using (var connection = _database.GetConnection())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                int nextId;
                using (var idCommand = connection.CreateCommand())
                {
                    idCommand.Transaction = transaction;
                    idCommand.CommandText = "SELECT ISNULL(MAX(Id), 0) + 1 FROM dbo.Bewijsstuk WITH (TABLOCKX, HOLDLOCK)";
                    nextId = Convert.ToInt32(idCommand.ExecuteScalar());
                }

                using var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO dbo.Bewijsstuk (Id, Titel, Soort, Datum) VALUES (@Id, @Titel, @Soort, @Datum)";
                command.Parameters.AddWithValue("@Id", nextId);
                command.Parameters.AddWithValue("@Titel", bewijsstuk.Titel.Trim());
                command.Parameters.AddWithValue("@Soort", bewijsstuk.Soort.Trim());
                command.Parameters.AddWithValue("@Datum", (object?)bewijsstuk.Datum ?? DBNull.Value);
                int rowsAffected = command.ExecuteNonQuery();
                transaction.Commit();
                bewijsstuk.Id = nextId;
                return rowsAffected > 0;
            }
        }

        public void VerwijderBewijsstuk(int bewijsstukId)
        {
            if (bewijsstukId <= 0)
            {
                throw new ArgumentException("Ongeldig bewijsstuk ID.", nameof(bewijsstukId));
            }
            using (var connection = _database.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM dbo.Bewijsstuk WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", bewijsstukId);
                    int rowsAffected = command.ExecuteNonQuery();
                    
                }
            }
        }

        public void WijzigBewijsstuk(BewijsstukDTO bewijsstuk)
        {
            ArgumentNullException.ThrowIfNull(bewijsstuk, nameof(bewijsstuk));
            if (bewijsstuk.Id <= 0)
            {
                throw new ArgumentException("Ongeldig bewijsstuk ID.", nameof(bewijsstuk.Id));
            }
            using (var connection = _database.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE dbo.Bewijsstuk SET Titel = @Titel, Soort = @Soort, Datum = @Datum WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", bewijsstuk.Id);
                    command.Parameters.AddWithValue("@Titel", bewijsstuk.Titel);
                    command.Parameters.AddWithValue("@Soort", bewijsstuk.Soort);
                    command.Parameters.AddWithValue("@Datum", (object?)bewijsstuk.Datum ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

}


