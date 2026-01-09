using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        private readonly ICriteriumRepository _criteriumRepository;
        
        public PrestatiedoelRepository(DbConnectieHelper dbConnectieHelper, ICriteriumRepository criteriumRepository) : base(dbConnectieHelper)
        {
            _criteriumRepository = criteriumRepository;
            
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Prestatiedoel (
                    prestatiedoel_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    niveau TEXT NOT NULL,
                    beschrijving TEXT NOT NULL,
                    ai_assessment_scale TEXT
                );
            ");
            
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS PrestatiedoelCriterium (
                    prestatiedoel_id INTEGER NOT NULL,
                    criterium_id INTEGER NOT NULL,
                    PRIMARY KEY (prestatiedoel_id, criterium_id),
                    FOREIGN KEY (prestatiedoel_id) REFERENCES Prestatiedoel(prestatiedoel_id),
                    FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
                );
            ");

            List<string> insertQueries =
            [
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, ai_assessment_scale)
                  VALUES ('OpNiveau',
                          'Maak een domeinmodel volgens een UML klassendiagram en leg deze vast in je plan en/of ontwerpdocumenten.',
                          'Samenwerking')",
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, ai_assessment_scale)
                  VALUES ('OpNiveau',
                          'Toepassen van modelleertechnieken door principes toe te passen volgens de ontwerprichtlijnen van HBO-ICT.',
                          'Samenwerking')"
            ];

            List<string> insertQueriesKoppeltabel =
            [
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 1)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 2)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 3)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (1, 5)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (2, 4)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriterium (prestatiedoel_id, criterium_id) VALUES (2, 6)"
            ];
            VoegMeerdereInMetTransactie(insertQueries);
            VoegMeerdereInMetTransactie(insertQueriesKoppeltabel);
        }

        public List<Prestatiedoel> HaalAllePrestatiedoelenOpMetCriteria()
        {
            List<Prestatiedoel> prestatiedoelen = HaalAllePrestatiedoelenOp();
            
            foreach (Prestatiedoel prestatiedoel in prestatiedoelen)
            {
                prestatiedoel.Criteria = _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoel.Id);
            }
            return prestatiedoelen;
        }
        
        public List<Prestatiedoel> HaalAllePrestatiedoelenOp()
        {
            List<Prestatiedoel> lijst = new List<Prestatiedoel>();

            OpenVerbinding();
            using SqliteCommand cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
                SELECT prestatiedoel_id, niveau, beschrijving, ai_assessment_scale
                FROM Prestatiedoel;
            ";

            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Prestatiedoel
                {
                    Id = reader.GetInt32(0),
                    Niveau = reader.GetString(1),
                    Beschrijving = reader.GetString(2),
                    AiAssessmentScale = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                });
            }

            SluitVerbinding();
            return lijst;
        }
    }
}