using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class LeeruitkomstRepository : DatabaseVerbinding, ILeeruitkomstRepository
    {
        public LeeruitkomstRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Leeruitkomst (
                    leeruitkomst_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(50),
                    hboi_activiteit VARCHAR(50))");

            List<string> insertQueries =
            [
                @"INSERT OR IGNORE INTO Leeruitkomst(leeruitkomst_id, naam, hboi_activiteit) 
                  VALUES(1, 'Analyseren', 'Analyseren')",
                @"INSERT OR IGNORE INTO Leeruitkomst(leeruitkomst_id, naam, hboi_activiteit) 
                  VALUES(2, 'Adviseren', 'Adviseren')",
                @"INSERT OR IGNORE INTO Leeruitkomst(leeruitkomst_id, naam, hboi_activiteit) 
                  VALUES(3, 'Ontwerpen', 'Ontwerpen')",
                @"INSERT OR IGNORE INTO Leeruitkomst(leeruitkomst_id, naam, hboi_activiteit) 
                  VALUES(4, 'Realiseren', 'Realiseren')",
                @"INSERT OR IGNORE INTO Leeruitkomst(leeruitkomst_id, naam, hboi_activiteit) 
                  VALUES(5, 'Manage & Control', 'Manage & Control')"
            ];
            VoegMeerdereInMetTransactie(insertQueries);

        }

        public Leeruitkomst? HaalOp()
        {
            List<Leeruitkomst> alleLeeruitkomsten = HaalAlleLeeruitkomstenOp();
            return alleLeeruitkomsten.FirstOrDefault();
        }

        public List<Leeruitkomst> HaalAlleLeeruitkomstenOp()
        {
            List<Leeruitkomst> leeruitkomsten = new();
            leeruitkomsten.Clear();
            string selectQuery = "SELECT leeruitkomst_id, naam, hboi_activiteit FROM Leeruitkomst";
            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int LeeruitkomstId = reader.GetInt32(0);
                    string LeeruitkomstNaam = reader.GetString(1);
                    string HboiActiviteit = reader.GetString(2);
                    leeruitkomsten.Add(new(LeeruitkomstId, LeeruitkomstNaam, HboiActiviteit));
                }
            }
            SluitVerbinding();
            return leeruitkomsten;
        }
    }
}