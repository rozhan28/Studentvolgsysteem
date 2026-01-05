using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ClusterRepository : DatabaseVerbinding, IClusterRepository
    {
        public ClusterRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Cluster (
                    cluster_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    naam VARCHAR(50),
                    code VARCHAR(50))");

            List<string> insertQueries = [@"INSERT OR REPLACE INTO Cluster(cluster_id, naam, code) 
                                        VALUES(1, 'OOSDD', 'OOSDD20252026')"];
            VoegMeerdereInMetTransactie(insertQueries);
        }

        public Cluster? HaalOp()
        {
            List<Cluster> alleClusters = HaalAlleClustersOp();
    
            // We retourneren even de eerste cluster uit de lijst, omdat er nog maar 1 is.
            // Waarschijnlijk zal er voorlopig ook maar 1 cluster zijn, maar deze methode alvast voor schaalbaarheid.
            return alleClusters.FirstOrDefault();
        }

        public List<Cluster> HaalAlleClustersOp()
        {
            List<Cluster> clusters = new();
            clusters.Clear();
            string selectQuery = "SELECT cluster_id, naam, code FROM Cluster";
            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int ClusterId = reader.GetInt32(0);
                    string ClusterNaam = reader.GetString(1);
                    string ClusterCode = reader.GetString(2);
                    clusters.Add(new(ClusterId, ClusterNaam, ClusterCode));
                }
            }
            SluitVerbinding();
            return clusters;
        }
    }
}