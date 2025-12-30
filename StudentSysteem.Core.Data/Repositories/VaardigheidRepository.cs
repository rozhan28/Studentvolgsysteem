using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class VaardigheidRepository : DatabaseVerbinding, IVaardigheidRepository
    {
        private readonly List<Vaardigheid> vaardigheidLijst = new();

        public VaardigheidRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Vaardigheid (
                    vaardigheid_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(255),
                    beschrijving TEXT,
                    leertaken_url VARCHAR(255),
                    prestatiedoel_id INTEGER,
                    processtap_id INTEGER,
                    leeruitkomst_id INTEGER,
                    FOREIGN KEY(prestatiedoel_id) REFERENCES Prestatiedoel(prestatiedoel_id),
                    FOREIGN KEY(leeruitkomst_id) REFERENCES Leeruitkomst(leeruitkomst_id))");

            List<string> VoegVaardigheid = [@"INSERT OR IGNORE INTO Vaardigheid
            (naam, beschrijving, leertaken_url, prestatiedoel_id, leeruitkomst_id)
            VALUES (
                'Maken domeinmodel',
                'Het maken van een domeinmodel volgens een UML klassendiagram.',
                'https://leertaken.nl/2.-Processen/1.-Requirementsanalyseproces/1.-Uitleg-defini%C3%ABren-probleemdomein',
                1,
                1)"];
            VoegMeerdereInMetTransactie(VoegVaardigheid);
        }

        public List<Vaardigheid> HaalAlleVaardighedenOp()
        {
            vaardigheidLijst.Clear();
            string selectQuery = "SELECT v.vaardigheid_id, v.naam, v.beschrijving, v.leertaken_url, v.prestatiedoel_id, v.leeruitkomst_id, l.hboi_activiteit FROM Vaardigheid v " +
                                 "LEFT JOIN Leeruitkomst l ON v.leeruitkomst_id = l.leeruitkomst_id";
            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Vaardigheid_id = reader.GetInt32(0);
                    string VaardigheidNaam = reader.GetString(1);
                    string VaardigheidBeschrijving = reader.GetString(2);
                    string LeertakenUrl = reader.GetString(3);
                    int PrestatiedoelId = reader.GetInt32(4);
                    int LeeruitkomstId = reader.GetInt32(5);
                    string HboiActiviteit = reader.GetString(6);
                    vaardigheidLijst.Add(new(Vaardigheid_id, VaardigheidNaam, VaardigheidBeschrijving, LeertakenUrl, PrestatiedoelId, LeeruitkomstId, HboiActiviteit));
                }
            }
            SluitVerbinding();
            return vaardigheidLijst;
        }
    }
}