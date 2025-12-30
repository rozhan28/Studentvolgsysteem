using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

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

            List<string> insertQueries = [@"INSERT OR REPLACE INTO Leeruitkomst(leeruitkomst_id, naam, hboi_activiteit) 
                                        VALUES(NULL, 'MockData', NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}