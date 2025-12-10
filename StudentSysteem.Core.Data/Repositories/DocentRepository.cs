using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class DocentRepository : DatabaseVerbinding, IDocentRepository
    {
        public DocentRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Docent (
                    [docent_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(50),
                    [email] VARCHAR(50),
                    [nummer] VARCHAR(50),
                    [cluster_id] INTEGER,
                    FOREIGN KEY([cluster_id]) REFERENCES Cluster(cluster_id))");

            List<string> insertQueries = [@"INSERT OR IGNORE INTO Docent(docent_id, naam, email, nummer, cluster_id) 
                                        VALUES(NULL, 'MockData2', NULL, NULL, NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
