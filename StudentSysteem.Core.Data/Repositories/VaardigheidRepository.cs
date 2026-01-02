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
            string selectQuery = "SELECT vaardigheid_id, naam, beschrijving, hboi_activiteit, leertaken_url, prestatiedoel_id FROM Vaardigheid";
            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Vaardigheid_id = reader.GetInt32(0);
                    string VaardigheidNaam = reader.GetString(1);
                    string VaardigheidBeschrijving = reader.GetString(2);
                    string HboiActiviteit = reader.GetString(3);
                    string LeertakenUrl = reader.GetString(4);
                    int PrestatiedoelId = reader.GetInt32(5);
                    vaardigheidLijst.Add(new(Vaardigheid_id, VaardigheidNaam, VaardigheidBeschrijving, HboiActiviteit, LeertakenUrl, PrestatiedoelId));
                }
            }
            SluitVerbinding();
            return vaardigheidLijst;
        }

    }
}
