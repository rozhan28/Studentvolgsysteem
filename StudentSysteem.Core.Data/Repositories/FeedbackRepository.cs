using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

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
                    [vaardigheid_id] INTEGER)");
            List<string> VoegFeedback = [
                @"INSERT OR REPLACE INTO Feedback(niveauaanduiding, toelichting, datum, tijd, student_id, docent_id, vaardigheid_id) 
            VALUES('1', 'NULL', NULL, NULL, NULL, NULL, NULL)"
            ];
            VoegMeerdereInMetTransactie(VoegFeedback);
        }

        public void VoegToelichtingToe(string toelichting, int studentId)
        {
            OpenVerbinding();
            using var transactie = Verbinding.BeginTransaction();
            try
            {
                using var cmd = Verbinding.CreateCommand();
                
                
                cmd.CommandText = @"INSERT INTO Feedback (toelichting, datum, tijd, student_id) 
                                    VALUES (@toelichting, @datum, @tijd, @studentId);";
                cmd.Parameters.AddWithValue("@toelichting", toelichting);
                var now = DateTime.Now;
                cmd.Parameters.AddWithValue("@datum", now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@tijd", now.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@studentId", studentId);
                cmd.Transaction = transactie;
                cmd.ExecuteNonQuery();

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
    }
}
