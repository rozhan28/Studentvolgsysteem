using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        public PrestatiedoelRepository()
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Prestatiedoel (
                    [processtap_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [niveau] VARCHAR(255),
                    [beschrijving] TEXT,
                    [criterium_id] INTEGER,
                    FOREIGN KEY([criterium_id]) REFERENCES Criterium(criterium_id))");

            List<string> insertQueries = [@"INSERT OR IGNORE INTO Prestatiedoel(processtap_id, niveau, beschrijving, criterium_id) 
                                        VALUES(NULL, 'MockData2', NULL, NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
