using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace StudentSysteem.Core.Data.Repositories
{
    public class FeedbackRepository : DatabaseVerbinding, IFeedbackRepository
    {
        public FeedbackRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            // Feedback tabel
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Feedback (
                    feedback_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    niveauaanduiding TEXT,
                    toelichting TEXT,
                    datum TEXT,
                    tijd TEXT,
                    student_id INTEGER,
                    docent_id INTEGER,
                    vaardigheid_id INTEGER
                );
            ");
        }

        /// Maakt een nieuwe feedback aan en geeft het feedbackId terug.
        public int MaakFeedbackAan(string niveau, int? studentId = null, int? docentId = null, int? vaardigheidId = null)
        {
            OpenVerbinding();
            using var cmd = Verbinding.CreateCommand();

            var now = DateTime.Now;

            cmd.CommandText = @"
                INSERT INTO Feedback (niveauaanduiding, datum, tijd, student_id, docent_id, vaardigheid_id)
                VALUES (@niveau, @datum, @tijd, @studentId, @docentId, @vaardigheidId);
                SELECT last_insert_rowid();
            ";

            cmd.Parameters.AddWithValue("@niveau", niveau ?? string.Empty);
            cmd.Parameters.AddWithValue("@datum", now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@tijd", now.ToString("HH:mm:ss"));
            cmd.Parameters.AddWithValue("@studentId", studentId.HasValue ? studentId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@docentId", docentId.HasValue ? docentId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@vaardigheidId", vaardigheidId.HasValue ? vaardigheidId.Value : (object)DBNull.Value);

            int feedbackId = Convert.ToInt32(cmd.ExecuteScalar());
            SluitVerbinding();

            return feedbackId;
        }
        
        /// Voegt meerdere toelichtingen toe voor één student (in transactie).
        public void VoegToelichtingenToe(List<Toelichting> toelichtingen, int studentId)
        {
            if (toelichtingen == null || toelichtingen.Count == 0) return;

            OpenVerbinding();
            using var transactie = Verbinding.BeginTransaction();

            try
            {
                foreach (var toelichting in toelichtingen)
                {
                    using var cmd = Verbinding.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO Feedback (toelichting, datum, tijd, student_id)
                        VALUES (@toelichting, @datum, @tijd, @studentId);
                    ";

                    var now = DateTime.Now;
                    cmd.Parameters.AddWithValue("@toelichting", toelichting.Tekst ?? string.Empty);
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

