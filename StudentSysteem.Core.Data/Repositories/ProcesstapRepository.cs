using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using System.Collections.Generic;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesstapRepository : DatabaseVerbinding, IProcesstapRepository
    {
        public ProcesstapRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            // Tabel Processtap
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Processtap (
                    processtap_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(255),
                    proces_id INTEGER,
                    criterium_id INTEGER,
                    FOREIGN KEY(proces_id) REFERENCES Proces(proces_id),
                    FOREIGN KEY(criterium_id) REFERENCES Criterium(criterium_id)
                );
            ");

            // Seed-data
            List<string> insertQueries = new()
            {
                @"INSERT OR IGNORE INTO Processtap (naam, proces_id) VALUES ('Definiëren probleemdomein', 1)",
                @"INSERT OR IGNORE INTO Processtap (naam, proces_id) VALUES ('Opstellen ontwerp', 2)"
            };

            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
