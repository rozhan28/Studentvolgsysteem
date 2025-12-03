using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesRepository : DatabaseVerbinding, IProcesRepository
    {
        public ProcesRepository()
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Proces (
                    [proces_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(50))");

            List<string> insertQueries = [@"INSERT OR IGNORE INTO Proces(proces_id, naam) 
                                        VALUES(NULL, 'MockData2')"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
