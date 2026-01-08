using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Models;
using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class CriteriumRepository : DatabaseVerbinding, ICriteriumRepository
    {
        public CriteriumRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"
            CREATE TABLE IF NOT EXISTS Criterium (
                criterium_id INTEGER PRIMARY KEY AUTOINCREMENT,
                beschrijving TEXT NOT NULL,
                niveau TEXT NOT NULL
            );");

            // Koppeltabel FeedbackCriterium
            MaakTabel(@"CREATE TABLE IF NOT EXISTS FeedbackCriterium (
                        feedback_id INTEGER NOT NULL,
                        criterium_id INTEGER NOT NULL,
                        PRIMARY KEY (feedback_id, criterium_id),
                        FOREIGN KEY (feedback_id) REFERENCES Feedback(feedback_id),
                        FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
                    );
                    ");
            
            List<string> insertQueries =
            [
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (1, 'De syntax van het domeinmodel is correct volgens UML.', 'OpNiveau')",
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (2, 'Het domeinmodel weerspiegelt het probleemgebied.', 'OpNiveau')",
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (3, 'Het domeinmodel is op een logische locatie vastgelegd.', 'OpNiveau')",
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (4, 'Modelleertechnieken dragen bij het overbrengen van het ontwerp.', 'OpNiveau')",
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (5, 'Het domeinmodel is volledig en logisch.', 'BovenNiveau')",
                @"INSERT OR IGNORE INTO Criterium (criterium_id, beschrijving, niveau)
                  VALUES (6, 'Diagrammen sluiten aan bij stakeholderbehoeften.', 'BovenNiveau')"
            ];
            VoegMeerdereInMetTransactie(insertQueries);
        }
        public List<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId)
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
            ";

            cmd.Parameters.AddWithValue("@prestatiedoelId", prestatiedoelId);

            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string beschrijving = reader.GetString(1);
                string niveauString = reader.GetString(2);
                
                if (!Enum.TryParse(niveauString, out Niveauaanduiding niveau))
                {
                    throw new Exception($"Onbekend niveau: {niveauString}");
                }

                lijst.Add(new Criterium(id, beschrijving, niveau));
            }

            SluitVerbinding();
            return lijst;
        }

        public void SlaGeselecteerdeCriteriaOp(int feedbackId, IEnumerable<Criterium> geselecteerdeCriteria)
        {
            OpenVerbinding();
            using SqliteTransaction transactie = Verbinding.BeginTransaction();

            try
            {
                foreach (Criterium criterium in geselecteerdeCriteria)
                {
                    using SqliteCommand cmd = Verbinding.CreateCommand();
                    cmd.CommandText = @"
                        INSERT OR REPLACE INTO FeedbackCriterium(feedback_id, criterium_id)
                        VALUES (@feedbackId, @criteriumId);
                        ";

                    cmd.Parameters.AddWithValue("@feedbackId", feedbackId);
                    cmd.Parameters.AddWithValue("@criteriumId", criterium.Id);
                    
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
