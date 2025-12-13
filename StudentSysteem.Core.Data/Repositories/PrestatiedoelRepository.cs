using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        public PrestatiedoelRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            // Hoofdtabel
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Prestatiedoel (
                    [processtap_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [niveau] VARCHAR(255),
                    [beschrijving] TEXT,
                    [criterium_id] INTEGER,
                    FOREIGN KEY([criterium_id]) REFERENCES Criterium(criterium_id))");

            // Koppeltabel 
            MaakTabel(@"CREATE TABLE IF NOT EXISTS PrestatiedoelCriteria (
                    prestatiedoel_id INTEGER,
                    criterium_id INTEGER,
                    PRIMARY KEY (prestatiedoel_id, criterium_id),
                    FOREIGN KEY (prestatiedoel_id) REFERENCES Prestatiedoel(processtap_id),
                    FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id))");

            // Init data
            List<string> insertQueries = [
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, criterium_id)
                  VALUES ('Op niveau', 'Maak een domeinmodel volgens een UML klassendiagram en leg deze vast in je plan en/of ontwerpdocumenten', 1)"
            ];

            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}

