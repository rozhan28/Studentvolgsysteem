using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;  
using System.Collections.Generic;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesRepository : DatabaseVerbinding, IProcesRepository
    {
        public ProcesRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Proces (
                    proces_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(50)
                );
            ");

            List<string> insertQueries = new()
            {
                @"INSERT OR IGNORE INTO Proces (proces_id, naam) VALUES (1, 'Requirementsanalyseproces')",
                @"INSERT OR IGNORE INTO Proces (proces_id, naam) VALUES (2, 'Ontwerpproces')"
            };

            VoegMeerdereInMetTransactie(insertQueries);
        }

        public IEnumerable<Proces> HaalAlleProcessenOp()
        {
            return VoerSelectUit(
                "SELECT proces_id, naam FROM Proces",
                r => new Proces
                {
                    Id = r.GetInt32(0),
                    Naam = r.GetString(1)
                });
        }
    }
}
