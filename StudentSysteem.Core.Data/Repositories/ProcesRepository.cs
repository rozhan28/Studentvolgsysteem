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
            // Tabel Proces
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Proces (
                    proces_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(50)
                );
            ");

            // Seed-data
            List<string> insertQueries = new()
            {
                @"INSERT OR IGNORE INTO Proces (proces_id, naam) VALUES (1, 'Requirementsanalyseproces')",
                @"INSERT OR IGNORE INTO Proces (proces_id, naam) VALUES (2, 'Ontwerpproces')"
            };
            VoegMeerdereInMetTransactie(insertQueries);


            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
