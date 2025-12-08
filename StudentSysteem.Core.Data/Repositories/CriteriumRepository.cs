using StudentSysteem.Core.Interfaces.Repository;
namespace StudentSysteem.Core.Data.Repositories
{
    public class CriteriumRepository : DatabaseVerbinding, ICriteriumRepository
    {
        public CriteriumRepository()
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Criterium (
                    [criterium_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [beschrijving] TEXT)");
        }
    }
}
