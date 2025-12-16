using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class CriteriumRepository : DatabaseVerbinding, ICriteriumRepository
    {
        public CriteriumRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            MaakTabel(@"
            CREATE TABLE IF NOT EXISTS Criterium (
                criterium_id INTEGER PRIMARY KEY AUTOINCREMENT,
                beschrijving TEXT
            )");

            List<string> seed = new()
        {
             //Op niveau
            @"INSERT OR REPLACE INTO Criterium (criterium_id, beschrijving)
              VALUES (1, 'De syntax van het domeinmodel is correct volgens UML')",

            @"INSERT OR REPLACE INTO Criterium (criterium_id, beschrijving)
              VALUES (2, 'Het domeinmodel is op een logische locatie vastgelegd')",

            //Boven niveau
            @"INSERT OR REPLACE INTO Criterium (criterium_id, beschrijving)
              VALUES (3, 'Het domeinmodel is volledig en logisch')"
        };

            VoegMeerdereInMetTransactie(seed);
        }
    }

}