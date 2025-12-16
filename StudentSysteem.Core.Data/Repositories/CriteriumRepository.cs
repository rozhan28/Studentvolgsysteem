using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using Microsoft.Data.Sqlite;

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
            beschrijving TEXT NOT NULL,
            niveau TEXT NOT NULL
        );");

            List<string> seed = new()
            {
                // Op niveau
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (1, 'De syntax van het domeinmodel is correct volgens UML', 'Op niveau')",

                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (2, 'Het domeinmodel weerspiegelt het probleemgebied', 'Op niveau')",

                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (3, 'Het domeinmodel is op een logische locatie vastgelegd', 'Op niveau')",

                // Boven niveau
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (4, 'Het domeinmodel is volledig en logisch', 'Boven niveau')"
            };


            VoegMeerdereInMetTransactie(seed);
        }

        public List<Criterium> HaalCriteriaOpVoorNiveau(string niveau)
        {
            var lijst = new List<Criterium>();

            OpenVerbinding();

            using var cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
        SELECT criterium_id, beschrijving
        FROM Criterium
        WHERE niveau = @niveau";

            cmd.Parameters.AddWithValue("@niveau", niveau);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Criterium
                {
                    Id = reader.GetInt32(0),
                    Beschrijving = reader.GetString(1),
                    Niveau = niveau
                });
            }

            SluitVerbinding();
            return lijst;
        }
    }

}