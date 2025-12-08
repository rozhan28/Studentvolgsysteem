using StudentSysteem.Core.Data.Helpers;
using Microsoft.Data.Sqlite;
using System.Data;

namespace StudentSysteem.Core.Data
{
    public abstract class DatabaseVerbinding : IDisposable
    {
        protected SqliteConnection Verbinding { get; }
        private string databaseNaam;

        public DatabaseVerbinding()
        {
            databaseNaam = "StepWiseDbs.sqlite";

            string projectMap = AppDomain.CurrentDomain.BaseDirectory ?? "";
            string padNaarDb = "Data Source=" + Path.Combine(projectMap, databaseNaam);

            Verbinding = new SqliteConnection(padNaarDb);
        }

        protected void OpenVerbinding()
        {
            if (Verbinding.State != ConnectionState.Open) Verbinding.Open();
        }

        protected void SluitVerbinding()
        {
            if (Verbinding.State != ConnectionState.Closed) Verbinding.Close();
        }

        public void MaakTabel(string sqlOpdracht)
        {
            OpenVerbinding();
            using (var command = Verbinding.CreateCommand())
            {
                command.CommandText = sqlOpdracht;
                command.ExecuteNonQuery();
            }
            SluitVerbinding();
        }

        protected void ExecuteNonQuery(string sql, SqliteTransaction? transaction = null)
        {
            using var command = Verbinding.CreateCommand();
            command.CommandText = sql;
            if (transaction != null) command.Transaction = transaction;
            command.ExecuteNonQuery();
        }

        public void VoegMeerdereInMetTransactie(List<string> regels)
        {
            OpenVerbinding();

            using var transactie = Verbinding.BeginTransaction();
            try
            {
                foreach (var r in regels)
                {
                    ExecuteNonQuery(r, transactie);
                }
                transactie.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try { transactie.Rollback(); } catch { /* ignore */ }
            }
            finally
            {
                SluitVerbinding();
            }
        }

        public void VerwijderInhoud(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name cannot be empty.", nameof(tableName));

            OpenVerbinding();
            try
            {
                using var command = Verbinding.CreateCommand();
                command.CommandText = $"DELETE FROM {tableName};";
                command.ExecuteNonQuery();
            }
            finally
            {
                SluitVerbinding();
            }
        }

        public void Dispose()
        {
            SluitVerbinding();
            Verbinding?.Dispose();
        }
    }
}
