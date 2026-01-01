using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using System.Collections.Generic;

namespace StudentSysteem.Core.Data.Repositories
{
    public class VaardigheidRepository : DatabaseVerbinding, IVaardigheidRepository
    {
        private readonly List<Vaardigheid> vaardigheidLijst = new();

        public VaardigheidRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            // Tabel Vaardigheid
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Vaardigheid (
                    vaardigheid_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(255),
                    beschrijving TEXT,
                    hboi_activiteit VARCHAR(255),
                    leertaken_url VARCHAR(255),
                    prestatiedoel_id INTEGER,
                    processtap_id INTEGER,
                    leeruitkomst_id INTEGER,
                    FOREIGN KEY(prestatiedoel_id) REFERENCES Prestatiedoel(prestatiedoel_id),
                    FOREIGN KEY(processtap_id) REFERENCES Processtap(processtap_id),
                    FOREIGN KEY(leeruitkomst_id) REFERENCES Leeruitkomst(leeruitkomst_id)
                );
            ");

            // Seed-data
            List<string> insertQueries = new()
            {
                @"INSERT OR IGNORE INTO Vaardigheid
                  (naam, beschrijving, hboi_activiteit, leertaken_url, prestatiedoel_id, processtap_id, leeruitkomst_id)
                  VALUES (
                    'Analyseren',
                    'Het maken van een domeinmodel volgens een UML klassendiagram',
                    'Maken domeinmodel',
                    'https://leertaken.nl/2.-Processen/1.-Requirementsanalyseproces/1.-Uitleg-defini%C3%ABren-probleemdomein',
                    1, 1, 1)",

                @"INSERT OR IGNORE INTO Vaardigheid
                  (naam, beschrijving, hboi_activiteit, leertaken_url, prestatiedoel_id, processtap_id, leeruitkomst_id)
                  VALUES (
                    'Ontwerpen',
                    'Modelleertechnieken toepassen conform richtlijnen',
                    'Toepassen modelleertechnieken',
                    'https://leertaken.nl/2.-Processen/3.-Ontwerpproces/2.-Uitleg-toepassen-modelleertechnieken',
                    2, 2, 2)"
            };

            VoegMeerdereInMetTransactie(insertQueries);
        }

        public List<Vaardigheid> HaalAlleVaardighedenOp()
        {
            vaardigheidLijst.Clear();

            string selectQuery = @"
                SELECT v.vaardigheid_id, v.naam, v.beschrijving, v.hboi_activiteit, v.leertaken_url, 
                       v.prestatiedoel_id, v.processtap_id, v.leeruitkomst_id
                FROM Vaardigheid v";

            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    vaardigheidLijst.Add(new Vaardigheid
                    {
                        Id = reader.GetInt32(0),
                        Naam = reader.GetString(1),
                        Beschrijving = reader.GetString(2),
                        HboiActiviteit = reader.IsDBNull(3) ? null : reader.GetString(3),
                        LeertakenUrl = reader.IsDBNull(4) ? null : reader.GetString(4),
                        PrestatiedoelId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        ProcesstapId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                        LeeruitkomstId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                    });
                }
            }

            SluitVerbinding();
            return vaardigheidLijst;
        }
    }
}
