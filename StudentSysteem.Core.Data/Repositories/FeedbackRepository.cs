using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

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
                niveauaanduiding VARCHAR(255),
                datum TEXT,
                tijd TEXT,
                student_id INTEGER,
                docent_id INTEGER,
                vaardigheid_id INTEGER,
                feedbackgever_id INTEGER,
                FOREIGN KEY (student_id) REFERENCES Student(vaardigheid_id),
                FOREIGN KEY (docent_id) REFERENCES Docent(docent_id),
                FOREIGN KEY (vaardigheid_id) REFERENCES Vaardigheid(vaardigheid_id),
                FOREIGN KEY (feedbackgever_id) REFERENCES Student(feedbackgever_id),
            )");

            MaakTabel(@"
            CREATE TABLE IF NOT EXISTS Toelichting (
                toelichting_id INTEGER PRIMARY KEY AUTOINCREMENT,
                toelichting TEXT NOT NULL,
                feedback_id INTEGER NOT NULL,
                criterium_id INTEGER NOT NULL,
                FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id),
                FOREIGN KEY (feedback_id) REFERENCES Feedback(feedback_id)
            )");
        }

        public void VoegFeedbackToe(List<Feedback> feedbackLijst)
        {
            OpenVerbinding();

            using var cmd = Verbinding.CreateCommand();
            using var transactie = Verbinding.BeginTransaction();
            var now = DateTime.Now;

            try
            {
                foreach (Feedback feedback in feedbackLijst)
            {
                cmd.CommandText = @"
                INSERT INTO Feedback 
                (niveauaanduiding, datum, tijd, student_id, docent_id, vaardigheid_id)
                VALUES (@niveau, @datum, @tijd, @studentId, @docentId, @vaardigheidId);
                SELECT last_insert_rowid();";

                cmd.Parameters.AddWithValue("@niveau", feedback.Niveauaanduiding.ToString());
                cmd.Parameters.AddWithValue("@datum", now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@tijd", now.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@studentId", feedback.StudentId);
                cmd.Parameters.AddWithValue("@docentId", feedback.DocentId);
                cmd.Parameters.AddWithValue("@vaardigheidId", feedback.VaardigheidId);

                int feedbackId = Convert.ToInt32(cmd.ExecuteScalar());

                {
                    foreach (Toelichting toelichting in feedback.Toelichtingen)
                    {
                        using var toelichtingCmd = Verbinding.CreateCommand();
                        toelichtingCmd.Transaction = transactie;

                        toelichtingCmd.CommandText = @"
                        INSERT INTO Toelichting 
                        (toelichting, feedback_id, criterium_id)
                        VALUES (@tekst, @feedbackId, @criteriumId);";

                        toelichtingCmd.Parameters.AddWithValue("@tekst", toelichting.Tekst);
                        toelichtingCmd.Parameters.AddWithValue("@feedbackId", feedbackId);
                        toelichtingCmd.Parameters.AddWithValue("@criteriumId",
                            0); //Hier moet nog de criterium op worden aangesloten

                        toelichtingCmd.ExecuteNonQuery();
                    }

                    transactie.Commit();
                }
            }
            }
            catch
            {
                transactie.Rollback();
                throw;
            }
            

            SluitVerbinding();
        }
    }
}
