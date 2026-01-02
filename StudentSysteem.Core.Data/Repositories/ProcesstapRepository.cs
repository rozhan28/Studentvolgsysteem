using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesstapRepository : DatabaseVerbinding, IProcesstapRepository
    {
        public ProcesstapRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
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

            List<string> insertQueries =
            [
                @"INSERT OR IGNORE INTO Processtap (processtap_id, naam, proces_id, criterium_id)
                  VALUES (1, 'Definiëren probleemdomein', 1, 1)",

                @"INSERT OR IGNORE INTO Processtap (processtap_id, naam, proces_id, criterium_id)
                  VALUES (2, 'Opstellen ontwerp', 2, 4)"
            ];
            VoegMeerdereInMetTransactie(insertQueries);
        }
        
        public IEnumerable<Processtap> HaalProcesstappenOpVoorProces(int procesId)
        {
            List<Processtap> processtappen = new();
            string selectQuery = "SELECT processtap_id, naam, proces_id, criterium_id FROM Processtap WHERE proces_id = @id";
    
            OpenVerbinding();
            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                command.Parameters.AddWithValue("@id", procesId);
                
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ProcesstapId = reader.GetInt32(0);
                        string ProcesstapNaam = reader.GetString(1);
                        int ProcesId = reader.GetInt32(2);
                        int CriteriumId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        processtappen.Add(new Processtap(ProcesstapId, ProcesstapNaam, ProcesId, CriteriumId));
                    }
                }
            }
            SluitVerbinding();
            return processtappen;
        }
    }
}