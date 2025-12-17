using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesRepository : DatabaseVerbinding, IProcesRepository
    {
        public ProcesRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Proces (
                    [proces_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(50))");

            List<string> insertQueries = [@"INSERT OR REPLACE INTO Proces (naam)
                                         VALUES ('Requirementsanalyseproces')"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}