using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class DocentRepository : DatabaseVerbinding, IDocentRepository
    {
        public DocentRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Docent (
                    docent_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(50),
                    email VARCHAR(50),
                    nummer INTEGER,
                    cluster_id INTEGER,
                    FOREIGN KEY(cluster_id) REFERENCES Cluster(cluster_id))");

            List<string> insertQueries = [@"INSERT OR REPLACE INTO Docent(docent_id, naam, email, nummer, cluster_id) 
                                        VALUES(1, 'Ernst', 'er.bolt@windesheim.nl', '123456', '1')"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
        
        public Docent? HaalOp()
        {
            List<Docent> alleDocenten = HaalAlleDocentenOp();
    
            // We retourneren even de eerste docent uit de lijst, omdat er nog maar 1 is.
            // Indien er meer docenten zijn en we hebben een inlogfunctie,
            // moeten we hier een WHERE email = ingevulde email check doen.
            return alleDocenten.FirstOrDefault();
        }

        public List<Docent> HaalAlleDocentenOp()
        {
            List<Docent> docenten = new();
            docenten.Clear();
            string selectQuery = "SELECT docent_id, naam, email, nummer, cluster_id FROM Docent";
            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int DocentId = reader.GetInt32(0);
                    string DocentNaam = reader.GetString(1);
                    string DocentEmail = reader.GetString(2);
                    int DocentNummer = reader.GetInt32(3);
                    int DocentClusterId = reader.GetInt32(4);
                    docenten.Add(new(DocentId, DocentNaam, DocentEmail, DocentNummer, DocentClusterId));
                }
            }
            SluitVerbinding();
            return docenten;
        }
    }
}