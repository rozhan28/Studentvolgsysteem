using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
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
                    naam TEXT NOT NULL,
                    proces_id INTEGER NOT NULL,
                    criterium_id INTEGER,
                    FOREIGN KEY(proces_id) REFERENCES Proces(proces_id),
                    FOREIGN KEY(criterium_id) REFERENCES Criterium(criterium_id)
                );
            ");

            // Seed-data
            List<string> insertQueries = new()
            {
                @"INSERT OR IGNORE INTO Processtap (processtap_id, naam, proces_id, criterium_id)
                  VALUES (1, 'Definiëren probleemdomein', 1, 1)",

                @"INSERT OR IGNORE INTO Processtap (processtap_id, naam, proces_id, criterium_id)
                  VALUES (2, 'Opstellen ontwerp', 2, 4)"
            };

            VoegMeerdereInMetTransactie(insertQueries);
        }

        public IEnumerable<Processtap> HaalProcesstappenOpVoorProces(int procesId)
        {
            return VoerSelectUit(
                @"SELECT processtap_id, naam 
                  FROM Processtap 
                  WHERE proces_id = @id
                  ORDER BY processtap_id",
                r => new Processtap
                {
                    Id = r.GetInt32(0),
                    Naam = r.GetString(1)
                },
                new Dictionary<string, object> { ["@id"] = procesId }
            );

        }
    }
}

