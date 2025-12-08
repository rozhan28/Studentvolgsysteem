using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class VaardigheidRepository : DatabaseVerbinding, IVaardigheidRepository
    {
        public VaardigheidRepository()
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Vaardigheid (
                    [vaardigheid_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(255),
                    [beschrijving] TEXT,
                    [hboi_competentie] VARCHAR(255),
                    [leertaken_url] VARCHAR(255),
                    [prestatiedoel_id] INTEGER,
                    [processtap_id] INTEGER,
                    FOREIGN KEY([prestatiedoel_id]) REFERENCES Prestatiedoel(prestatiedoel_id),
                    FOREIGN KEY([processtap_id]) REFERENCES Processtap(processtap_id))");

            List<string> insertQueries = [@"INSERT OR IGNORE INTO Vaardigheid(vaardigheid_id, naam, beschrijving, hboi_competentie, leertaken_url, prestatiedoel_id, processtap_id) 
                                        VALUES(NULL, 'MockData2', NULL, NULL, NULL, NULL, NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}
