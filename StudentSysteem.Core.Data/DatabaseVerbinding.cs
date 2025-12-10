using StudentSysteem.Core.Data.Helpers;
using Microsoft.Data.Sqlite;
using System.Data;
using Microsoft.Maui.Storage;

namespace StudentSysteem.Core.Data
{
    public abstract class DatabaseVerbinding : IDisposable
    {
        protected SqliteConnection Verbinding { get; }
        private string databaseBestandsnaam;

        public DatabaseVerbinding(DbConnectieHelper dbConnectieHelper)
        {
            databaseBestandsnaam = dbConnectieHelper.ConnectionStringValue("StepWiseDb");
            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(baseDir, databaseBestandsnaam);
            string dbConnection = $"Data Source={dbPath}; Foreign Keys=True; Pooling=False; Cache=Shared";
            Verbinding = new SqliteConnection(dbConnection);

            /*
            // OPTIONEEL: Zorg ervoor dat de database bestaat als deze voor de eerste keer wordt gestart
            // Dit is een goede gewoonte bij SQLite
            if (!File.Exists(dbPath))
            {
                // Roep hier logica aan om de database en tabellen aan te maken.
                // U kunt dit het beste in de klasse die dit erft regelen.
            }
            */
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
        }

        /*
        protected void ExecuteNonQuery(string sql, SqliteTransaction? transaction = null)
        {
            using var command = Verbinding.CreateCommand();
            command.CommandText = sql;
            if (transaction != null) command.Transaction = transaction;
            command.ExecuteNonQuery();
        }
        */

        public void VoegMeerdereInMetTransactie(List<string> regels)
        {
            OpenVerbinding();
            var transactie = Verbinding.BeginTransaction();

            try
            {
                using var cmd = Verbinding.CreateCommand();
                cmd.Transaction = transactie; 
        
                foreach (var sql in regels)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery(); // Nu werkt het gegarandeerd met de transactie
                }
        
                transactie.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try { transactie.Rollback(); } catch { /* Ignore secondary error */ }
                throw;
            }
            finally
            {
                SluitVerbinding();
            }
        }

        public void Dispose()
        {
            SluitVerbinding();
            Verbinding.Dispose();
        }
    }
}