using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class FeedbackRepository : DatabaseVerbinding, IFeedbackRepository
    {
        public FeedbackRepository()
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Feedback (
                    [feedback_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [niveauaanduiding] VARCHAR(255),
                    [toelichting] TEXT,
                    [datum] DATE,
                    [tijd] TIME,
                    [student_id] INTEGER,
                    [docent_id] INTEGER,
                    [vaardigheid_id] INTEGER)");

            
        }
    }
}