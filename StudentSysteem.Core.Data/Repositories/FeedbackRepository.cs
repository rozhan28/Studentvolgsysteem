using System.Diagnostics;
using Microsoft.Data.Sqlite;
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
            MaakTabel("PRAGMA foreign_keys = ON;");
            
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
                FOREIGN KEY (student_id) REFERENCES Student(student_id),
                FOREIGN KEY (docent_id) REFERENCES Docent(docent_id),
                FOREIGN KEY (vaardigheid_id) REFERENCES Vaardigheid(vaardigheid_id),
                FOREIGN KEY (feedbackgever_id) REFERENCES Student(student_id)
            )");

            MaakTabel(@"
            CREATE TABLE IF NOT EXISTS Toelichting (
                toelichting_id INTEGER PRIMARY KEY AUTOINCREMENT,
                toelichting TEXT,
                feedback_id INTEGER NOT NULL,
                criterium_id INTEGER,
                FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id),
                FOREIGN KEY (feedback_id) REFERENCES Feedback(feedback_id)
            )");
            
            MaakTabel(@"
            CREATE TABLE IF NOT EXISTS FeedbackCriterium (
                        feedback_id INTEGER NOT NULL,
                        criterium_id INTEGER NOT NULL,
                        PRIMARY KEY (feedback_id, criterium_id),
                        FOREIGN KEY (feedback_id) REFERENCES Feedback(feedback_id),
                        FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
            )");
        }
        
        public void VoegFeedbackToe(List<Feedback> feedbackLijst)
        {
            OpenVerbinding();
            
            using SqliteTransaction transactie = Verbinding.BeginTransaction();
            DateTime now = DateTime.Now;

            try
            {
                //Feedback opslaan
                foreach (Feedback feedback in feedbackLijst)
                {
                    using SqliteCommand cmd = Verbinding.CreateCommand();
                    cmd.Transaction = transactie;
                    
                    cmd.CommandText = @"
                    INSERT INTO Feedback 
                    (niveauaanduiding, datum, tijd, student_id, docent_id, vaardigheid_id, feedbackgever_id)
                    VALUES (@niveau, @datum, @tijd, @studentId, @docentId, @vaardigheidId, @feedbackgeverId);
                    SELECT last_insert_rowid();";
                    
                    cmd.Parameters.AddWithValue("@niveau", feedback.Niveauaanduiding.ToString());
                    cmd.Parameters.AddWithValue("@datum", now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@tijd", now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@studentId", feedback.StudentId);
                    
                    if (feedback.DocentId != 0) { cmd.Parameters.AddWithValue("@docentId", feedback.DocentId); } 
                    else { cmd.Parameters.AddWithValue("@docentId", DBNull.Value); }
                    
                    if (feedback.FeedbackGeverId != 0) { cmd.Parameters.AddWithValue("@feedbackgeverId", feedback.FeedbackGeverId); } 
                    else { cmd.Parameters.AddWithValue("@feedbackgeverId", DBNull.Value); }
                    
                    cmd.Parameters.AddWithValue("@vaardigheidId", feedback.VaardigheidId);

                    int feedbackId = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    // Geselecteerde criteria opslaan
                    foreach (Criterium criterium in feedback.Criteria)
                    {
                        Debug.WriteLine(criterium.Beschrijving);
                        using SqliteCommand criteriaCmd = Verbinding.CreateCommand();
                        criteriaCmd.Transaction = transactie;
                        
                        criteriaCmd.CommandText = @"
                        INSERT INTO FeedbackCriterium 
                        (feedback_id, criterium_id)
                        VALUES (@feedbackId, @criteriumId);";
                        
                        criteriaCmd.Parameters.AddWithValue("@feedbackId", feedbackId);
                        criteriaCmd.Parameters.AddWithValue("@criteriumId", criterium.Id);
                        
                        criteriaCmd.ExecuteNonQuery();
                    }

                    // Toelichtingen opslaan
                    foreach (Toelichting toelichting in feedback.Toelichtingen)
                    {
                        using SqliteCommand toelichtingCmd = Verbinding.CreateCommand();
                        toelichtingCmd.Transaction = transactie;

                        toelichtingCmd.CommandText = @"
                        INSERT INTO Toelichting 
                        (toelichting, feedback_id, criterium_id)
                        VALUES (@tekst, @feedbackId, @criteriumId);";

                        toelichtingCmd.Parameters.AddWithValue("@tekst", toelichting.Tekst);
                        toelichtingCmd.Parameters.AddWithValue("@feedbackId", feedbackId);
                        
                        if (toelichting.GeselecteerdeOptie.Id != 0) { toelichtingCmd.Parameters.AddWithValue("@criteriumId", toelichting.GeselecteerdeOptie.Id); }
                        else { toelichtingCmd.Parameters.AddWithValue("@criteriumId", DBNull.Value); }

                        toelichtingCmd.ExecuteNonQuery();
                    }
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
