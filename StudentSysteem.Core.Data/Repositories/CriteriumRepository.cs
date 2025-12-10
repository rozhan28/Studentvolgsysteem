using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
namespace StudentSysteem.Core.Data.Repositories
{
    public class CriteriumRepository : DatabaseVerbinding, ICriteriumRepository
    {
        public CriteriumRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Criterium (
                    [criterium_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [beschrijving] TEXT)");
            List<string> VoegCriterium = [
                @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(1, 'Het domeinmodel weerspiegelt de belangrijke onderdelen van het domein')",
                @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(2, 'De syntax van het domeinmodel is correct volgens UML')",
                @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(3, 'De syntax van het domeinmodel is correct volgens UML')",
                @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(4, ' Het domeinmodel is volledig, helder en sluit logisch aan bij de context van het project')"
            ];
            VoegMeerdereInMetTransactie(VoegCriterium);
        }
    }
}
