using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class ProcesstapRepository : DatabaseVerbinding, IProcesstapRepository
    {
        public ProcesstapRepository()
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Processtap (
                    [processtap_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(255),
                    [proces_id] INTEGER,
                    [criterium_id] INTEGER,
                    FOREIGN KEY([proces_id]) REFERENCES Proces(proces_id),
                    FOREIGN KEY([criterium_id]) REFERENCES Criterium(criterium_id))");

            List<string> insertQueries = [@"INSERT OR IGNORE INTO Processtap(processtap_id, naam, proces_id, criterium_id) 
                                        VALUES(NULL, 'MockData2', NULL, NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
