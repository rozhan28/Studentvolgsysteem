using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        public PrestatiedoelRepository(
            DbConnectieHelper dbConnectieHelper,
            ICriteriumRepository criteriumRepository)
            : base(dbConnectieHelper)
        {
            // Prestatiedoel
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Prestatiedoel (
                    prestatiedoel_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    niveau TEXT NOT NULL,
                    beschrijving TEXT NOT NULL,
                    criterium_id INTEGER,
                    ai_assessment_scale TEXT,
                    FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
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

            // ===== Seed Prestatiedoelen (US3) =====
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

            // ===== Seed koppelingen (US3) =====
            List<string> seedKoppeling = new()
            {
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 1)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 2)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 3)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 5)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (2, 4)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (2, 6)"
            };

            // ===== Seed AI / develop =====
            List<string> seedAi = new()
            {
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, criterium_id, ai_assessment_scale)
                  VALUES (
                    'Op niveau',
                    'Maak een domeinmodel volgens een UML klassendiagram en leg deze vast in je plan en/of ontwerpdocumenten.',
                    1,
                    'Samenwerking'
                  )"
            };

            VoegMeerdereInMetTransactie(seedPrestatiedoel);
            VoegMeerdereInMetTransactie(seedKoppeling);
            VoegMeerdereInMetTransactie(seedAi);
        }

        public List<Prestatiedoel> HaalAllePrestatiedoelenOp()
        {
            var lijst = new List<Prestatiedoel>();

            OpenVerbinding();
            using var cmd = Verbinding.CreateCommand();

            cmd.CommandText = @"
                SELECT 
                    prestatiedoel_id,
                    niveau,
                    beschrijving,
                    criterium_id,
                    ai_assessment_scale
                FROM Prestatiedoel;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Prestatiedoel
                {
                    Id = reader.GetInt32(0),
                    Niveau = reader.GetString(1),
                    Beschrijving = reader.GetString(2),
                    CriteriumId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    AiAssessmentScale = reader.IsDBNull(4) ? null : reader.GetString(4)
                });
            }

            SluitVerbinding();
            return lijst;
        }
    }
}
