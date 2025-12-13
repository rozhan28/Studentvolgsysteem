using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using Microsoft.Data.Sqlite;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        public PrestatiedoelRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Prestatiedoel (
                [processtap_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                [niveau] VARCHAR(255),
                [beschrijving] TEXT,
                [criterium_id] INTEGER,
                FOREIGN KEY([criterium_id]) REFERENCES Criterium(criterium_id)
            )");

            List<string> insertQueries =
            [
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, criterium_id)
                  VALUES (
                      'Op niveau',
                      'Maak een domeinmodel volgens een UML klassendiagram en leg deze vast in je plan en/of ontwerpdocumenten',
                      1
                  )"
            ];

            VoegMeerdereInMetTransactie(insertQueries);
        }

        public List<Prestatiedoel> HaalAllePrestatiedoelenOp()
        {
            List<Prestatiedoel> prestatiedoelen = new();

            try
            {
                OpenVerbinding(); 

                using var cmd = Verbinding.CreateCommand();
                cmd.CommandText = @"
            SELECT processtap_id, niveau, beschrijving, criterium_id
            FROM Prestatiedoel
        ";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prestatiedoelen.Add(new Prestatiedoel
                    {
                        Id = reader.GetInt32(0),
                        Niveau = reader.GetString(1),
                        Beschrijving = reader.GetString(2),
                        CriteriumId = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                    });
                }
            }
            finally
            {
                SluitVerbinding(); 
            }

            return prestatiedoelen;
        }

    }
}
