using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using Microsoft.Data.Sqlite;

namespace StudentSysteem.Core.Data.Repositories
{
    public class FeedbackRepository : DatabaseVerbinding, IFeedbackRepository
    {
        public FeedbackRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Feedback (
                    feedback_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    niveauaanduiding TEXT NOT NULL,
                    toelichting TEXT,
                    datum TEXT,
                    tijd TEXT,
                    student_id INTEGER,
                    docent_id INTEGER,
                    vaardigheid_id INTEGER
                );
            ");

            // Seed uit develop-branch behouden
            List<string> voegFeedback = new()
            {
                @"INSERT OR IGNORE INTO Feedback
                  (feedback_id, niveauaanduiding, toelichting, datum, tijd, student_id, docent_id, vaardigheid_id)
                  VALUES (1, '1', NULL, NULL, NULL, NULL, NULL, NULL)"
            };

            VoegMeerdereInMetTransactie(voegFeedback);
        }

        // ===== Feature/US3 =====
        public int MaakFeedbackAan(string niveau)
        {
            OpenVerbinding();

            using var cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Feedback (niveauaanduiding, datum, tijd)
                VALUES (@niveau, @datum, @tijd);
                SELECT last_insert_rowid();
            ";

            cmd.Parameters.AddWithValue("@niveau", niveau);

            var now = DateTime.Now;
            cmd.Parameters.AddWithValue("@datum", now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@tijd", now.ToString("HH:mm:ss"));

            int feedbackId = Convert.ToInt32(cmd.ExecuteScalar());

            SluitVerbinding();
            return feedbackId;
        }

        // ===== Feature/US3 =====
        public void VoegToelichtingToe(int feedbackId, string toelichting)
        {
            OpenVerbinding();

            using var cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
                UPDATE Feedback
                SET toelichting = @toelichting
                WHERE feedback_id = @feedbackId;
            ";

            cmd.Parameters.AddWithValue("@feedbackId", feedbackId);
            cmd.Parameters.AddWithValue("@toelichting", toelichting);

            cmd.ExecuteNonQuery();
            SluitVerbinding();
        }

        // ===== Develop =====
        public void VoegToelichtingenToe(List<Toelichting> toelichtingen, int studentId)
        {
            OpenVerbinding();
            using var transactie = Verbinding.BeginTransaction();

            try
            {
                foreach (Toelichting toelichting in toelichtingen)
                {
                    using var cmd = Verbinding.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO Feedback (toelichting, datum, tijd, student_id)
                        VALUES (@toelichting, @datum, @tijd, @studentId);
                    ";

                    cmd.Parameters.AddWithValue("@toelichting", toelichting.Tekst);

                    var now = DateTime.Now;
                    cmd.Parameters.AddWithValue("@datum", now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@tijd", now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@studentId", studentId);

                    cmd.Transaction = transactie;
                    cmd.ExecuteNonQuery();
                }

                transactie.Commit();
            }
            catch
            {
                try { transactie.Rollback(); } catch { }
                throw;
            }
            finally
            {
                SluitVerbinding();
            }
        }
    }
}

