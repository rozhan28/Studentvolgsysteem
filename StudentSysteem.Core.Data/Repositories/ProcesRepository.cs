using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using System.Collections.Generic;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesRepository : DatabaseVerbinding, IProcesRepository
    {
        public ProcesRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            // Proces tabel
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Proces (
                    proces_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(50)
                );
            ");

            // Seed data
            List<string> insertQueries = new()
            {
                @"INSERT OR IGNORE INTO Proces (naam) VALUES ('Requirementsanalyseproces')",
                @"INSERT OR IGNORE INTO Proces (naam) VALUES ('Ontwerpproces')"
            };

            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}

