using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
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
        }

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
    }
}
