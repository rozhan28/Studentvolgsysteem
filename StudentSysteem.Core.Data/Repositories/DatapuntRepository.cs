using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories;

public class DatapuntRepository : DatabaseVerbinding, IDatapuntRepository
{
    public DatapuntRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
    {
        MaakTabel(@"CREATE TABLE IF NOT EXISTS Datapunt (
                    [datapunt_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(255)");
    }

    public List<Datapunt> HaalAlleDatapuntenOpVanStudent(int StudentId)
    {
        List<Datapunt> datapunten = new();
        return datapunten;
    }
}