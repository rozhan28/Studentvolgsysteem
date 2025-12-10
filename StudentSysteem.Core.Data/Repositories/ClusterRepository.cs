using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ClusterRepository : DatabaseVerbinding, IClusterRepository
    {
        public ClusterRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Cluster (
                    [cluster_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(50),
                    [code] VARCHAR(50))");

            List<string> insertQueries = [@"INSERT OR IGNORE INTO Cluster(cluster_id, naam, code) 
                                        VALUES(NULL, 'MockData2', NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
