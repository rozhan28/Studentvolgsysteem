using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        public PrestatiedoelRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            // Prestatiedoel
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Prestatiedoel (
                    prestatiedoel_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    niveau TEXT NOT NULL,
                    beschrijving TEXT NOT NULL
                );
            ");

            // Koppeltabel
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS PrestatiedoelCriterium (
                    prestatiedoel_id INTEGER NOT NULL,
                    criterium_id INTEGER NOT NULL,
                    PRIMARY KEY (prestatiedoel_id, criterium_id),
                    FOREIGN KEY (prestatiedoel_id) REFERENCES Prestatiedoel(prestatiedoel_id),
                    FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
                );
            ");

            List<string> seedPrestatiedoel = new()
            {
                @"INSERT OR IGNORE INTO Prestatiedoel (prestatiedoel_id, niveau, beschrijving)
                  VALUES (
                    1,
                    'Op niveau',
                    'Maak een domeinmodel volgens een UML klassendiagram en leg deze vast in je plan en/of ontwerpdocumenten'
                  )",

                @"INSERT OR IGNORE INTO Prestatiedoel (prestatiedoel_id, niveau, beschrijving)
                  VALUES (
                    2,
                    'Op niveau',
                    'Toepassen van modelleertechnieken door principes toe te passen volgens de ontwerprichtlijnen van HBO-ICT.'
                  )"
            };

            List<string> seedKoppeling = new()
            {
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 1)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 2)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 3)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 5)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (2, 4)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (2, 6)"
            };

            VoegMeerdereInMetTransactie(seedPrestatiedoel);
            VoegMeerdereInMetTransactie(seedKoppeling);
        }

        public List<Prestatiedoel> HaalAllePrestatiedoelenOp()
        {
            var lijst = new List<Prestatiedoel>();

            OpenVerbinding();
            using var cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
                SELECT prestatiedoel_id, niveau, beschrijving
                FROM Prestatiedoel;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Prestatiedoel
                {
                    Id = reader.GetInt32(0),
                    Niveau = reader.GetString(1),
                    Beschrijving = reader.GetString(2)
                });
            }

            SluitVerbinding();
            return lijst;
        }
    }
}
