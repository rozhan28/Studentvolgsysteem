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

            // Koppeltabel 
            MaakTabel(@"CREATE TABLE IF NOT EXISTS FeedbackCriterium (
                        feedback_id INTEGER NOT NULL,
                        criterium_id INTEGER NOT NULL,
                        niveau TEXT NOT NULL,
                        PRIMARY KEY (feedback_id, criterium_id),
                        FOREIGN KEY (feedback_id) REFERENCES Feedback(feedback_id),
                        FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
                    );
                    ");

            List<string> seed = new()
            {
                // Op niveau
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (1, 'De syntax van het domeinmodel is correct volgens UML', 'Op niveau')",

                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (2, 'Het domeinmodel weerspiegelt het probleemgebied', 'Op niveau')",

                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (3, 'Het domeinmodel is op een logische locatie vastgelegd', 'Op niveau')",

                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (4, 'Modelleertechnieken dragen bij het overbrengen van het ontwerp', 'Op niveau')",

                // Boven niveau
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (5, 'Het domeinmodel is volledig en logisch', 'Boven niveau')",

                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau) 
                  VALUES (6, 'Diagrammen sluiten aan bij stakeholderbehoeften', 'Boven niveau')"
            };

            VoegMeerdereInMetTransactie(seed);
        }

        public List<Criterium> HaalCriteriaOpVoorNiveau(string niveau)
        {
            List<Criterium> lijst = new();

            OpenVerbinding();

            using SqliteCommand cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
                SELECT criterium_id, beschrijving, niveau
                FROM Criterium
                WHERE niveau = @niveau;
            ";

            cmd.Parameters.AddWithValue("@niveau", niveau);

            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Criterium
                {
                    Id = reader.GetInt32(0),
                    Beschrijving = reader.GetString(1),
                    Niveau = reader.GetString(2)
                });
            }

            SluitVerbinding();
            return lijst;
        }


        public List<Criterium> HaalCriteriaOpVoorPrestatiedoel(
            int prestatiedoelId,
            string niveau)
        {
            List<Criterium> lijst = new List<Criterium>();

            OpenVerbinding();

            using SqliteCommand cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
                SELECT c.criterium_id, c.beschrijving, c.niveau
                FROM Criterium c
                JOIN PrestatiedoelCriterium pc
                    ON pc.criterium_id = c.criterium_id
                WHERE pc.prestatiedoel_id = @prestatiedoelId
                  AND c.niveau = @niveau;
            ";

            cmd.Parameters.AddWithValue("@prestatiedoelId", prestatiedoelId);
            cmd.Parameters.AddWithValue("@niveau", niveau);

            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Criterium
                {
                    Id = reader.GetInt32(0),
                    Beschrijving = reader.GetString(1),
                    Niveau = reader.GetString(2)
                });
            }

            SluitVerbinding();
            return lijst;
        }



        public void SlaGeselecteerdeCriteriaOp(
            int feedbackId,
            IEnumerable<Criterium> geselecteerdeCriteria,
            string niveau)
        {
            OpenVerbinding();
            using var transactie = Verbinding.BeginTransaction();

            try
            {
                foreach (var criterium in geselecteerdeCriteria)
                {
                    using var cmd = Verbinding.CreateCommand();
                    cmd.CommandText = @"
                        INSERT OR REPLACE INTO FeedbackCriterium
                        (feedback_id, criterium_id, niveau)
                        VALUES (@feedbackId, @criteriumId, @niveau);
                        ";

                    cmd.Parameters.AddWithValue("@feedbackId", feedbackId);
                    cmd.Parameters.AddWithValue("@criteriumId", criterium.Id);
                    cmd.Parameters.AddWithValue("@niveau", niveau);
                    
                    cmd.Transaction = transactie;
                    cmd.ExecuteNonQuery();
                }

                transactie.Commit();
            }
            catch
            {
                transactie.Rollback();
                throw;
            }
            finally
            {
                SluitVerbinding();
            }
        }

    }
}