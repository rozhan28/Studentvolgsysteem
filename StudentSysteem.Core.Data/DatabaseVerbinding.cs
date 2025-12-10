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
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, databaseBestandsnaam);
            string dbConnection = $"Data Source={dbPath}";
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
                regels.ForEach(l => Verbinding.ExecuteNonQuery(l));
                transactie.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    transactie.Rollback();
                }
                finally
                {
                    transactie.Dispose();
                }
            }
        }

        public void Dispose()
        {
            SluitVerbinding();
        }
    }
}