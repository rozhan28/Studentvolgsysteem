using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class FeedbackRepository : DatabaseVerbinding, IFeedbackRepository
    {
        public FeedbackRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Feedback (
                    [feedback_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [niveauaanduiding] VARCHAR(255),
                    [toelichting] TEXT,
                    [datum] TEXT,
                    [tijd] TEXT,
                    [student_id] INTEGER,
                    [docent_id] INTEGER,
                    [vaardigheid_id] INTEGER,
                    [datapunt_id] INTEGER)");
            List<string> VoegFeedback = [
                @"INSERT OR IGNORE INTO Feedback(niveauaanduiding, toelichting, datum, tijd, student_id, docent_id, vaardigheid_id, datapunt_id) 
            VALUES('1', 'NULL', NULL, NULL, NULL, NULL, NULL, NULL)"
            ];
            VoegMeerdereInMetTransactie(VoegFeedback);
        }

        public void VoegMeerdereInMetTransactie(List<string> regels)
        {
            base.VoegMeerdereInMetTransactie(regels);
        }

        public void VoegToelichtingenToe(List<Toelichting> toelichtingen, int studentId)
        {
            OpenVerbinding();
            using var transactie = Verbinding.BeginTransaction();
            try
            {
                foreach (Toelichting toelichting in toelichtingen)
                {
                    using var cmd = Verbinding.CreateCommand();


                    cmd.CommandText = @"INSERT INTO Feedback (toelichting, datum, tijd, student_id) 
                                    VALUES (@toelichting, @datum, @tijd, @studentId);";
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try { transactie.Rollback(); } catch { }
                throw;
            }
            finally
            {
                SluitVerbinding();
            }
        }

        public List<Feedback> HaalFeedbackOp(Datapunt datapunt, int studentId)
        {
            List<Feedback> feedbackLijst = new();
            string selectQuery = @"SELECT feedback_id, niveauaanduiding, toelichting, datum, tijd, student_id, docent_id, vaardigheid_id, datapunt_id
                                FROM Feedback WHERE datapunt_id = @datapuntId AND student_id = @studentId";
            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                
                command.Parameters.AddWithValue("@datapuntId", datapunt.Datapunt_id);
                command.Parameters.AddWithValue("@studentId", studentId);

                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int feedbackId = reader.GetInt32(0);
                    string niveauaanduiding = reader.GetString(1);
                    string toelichting = reader.GetString(2);
                    string datum = reader.GetString(3);
                    string tijd = reader.GetString(4);
                    int _studentId = reader.GetInt32(5);
                    int docentId = reader.GetInt32(6);
                    int vaardigheidId = reader.GetInt32(7);
                    int datapuntId = reader.GetInt32(8);
                    feedbackLijst.Add(new(feedbackId, niveauaanduiding, toelichting, datum, tijd, _studentId, docentId, vaardigheidId, datapuntId));
                }
            }
            SluitVerbinding();
            return feedbackLijst;
        }
    }
}
