using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class LeeruitkomstRepository : DatabaseVerbinding, ILeeruitkomstRepository
    {
        public LeeruitkomstRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Leeruitkomst (
                    [leeruitkomst_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(50),
                    [hboi_activiteit] VARCHAR(50))");

            List<string> insertQueries = [@"INSERT OR REPLACE INTO Leeruitkomst(leeruitkomst_id, naam, hboi_activiteit) 
                                        VALUES(NULL, 'MockData2', NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
        
        public List<Leeruitkomst> HaalAlleLeeruitkomstenOp()
        {
            List<Leeruitkomst> lijst = new();

            OpenVerbinding();
            using var cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"SELECT leeruitkomst_id, naam, hboi_activiteit FROM Leeruitkomst";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Leeruitkomst
                {
                    Leeruitkomst_id = reader.GetInt32(0),
                    LeeruitkomstNaam = reader.GetString(1),
                    HboiActiviteit = reader.GetString(2),
                });
            }

            SluitVerbinding();
            return lijst;
        }
    }
}