using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesRepository : DatabaseVerbinding, IProcesRepository
    {
        public ProcesRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Proces (
                    proces_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(50)
                );
            ");

            List<string> insertQueries =
            [
                @"INSERT OR IGNORE INTO Proces (proces_id, naam) VALUES (1, 'Requirementsanalyseproces')",
                @"INSERT OR IGNORE INTO Proces (proces_id, naam) VALUES (2, 'Ontwerpproces')"
            ];
            VoegMeerdereInMetTransactie(insertQueries);
        }
        
        public Proces? HaalOp(int procesId)
        {
            Proces? proces = null;
            string selectQuery = "SELECT proces_id, naam FROM Proces WHERE proces_id = @id";

            OpenVerbinding();
            using (SqliteCommand command = new SqliteCommand(selectQuery, Verbinding))
            {
                command.Parameters.AddWithValue("@id", procesId);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string naam = reader.GetString(1);
                        proces = new Proces(id, naam);
                    }
                }
            }
            SluitVerbinding();
            return proces;
        }
    }
}