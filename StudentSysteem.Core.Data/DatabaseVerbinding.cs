using StudentSysteem.Core.Data.Helpers;
using Microsoft.Data.Sqlite;

namespace StudentSysteem.Core.Data
{
    public abstract class DatabaseVerbinding : IDisposable
    {
        protected SqliteConnection Verbinding { get; }
        private string databaseNaam;

        public DatabaseVerbinding()
        {
            databaseNaam = "StepWiseDbs.sqlite";

            string projectMap = AppDomain.CurrentDomain.BaseDirectory;
            string padNaarDb = "Data Source=" + Path.Combine(projectMap + databaseNaam);

            Verbinding = new SqliteConnection(padNaarDb);
        }

        protected void OpenVerbinding()
        {
            if (Verbinding.State != System.Data.ConnectionState.Open) Verbinding.Open();
        }

        protected void SluitVerbinding()
        {
            if (Verbinding.State != System.Data.ConnectionState.Closed) Verbinding.Close();
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

        public void VoegMeerdereInMetTransactie(List<string> regels)
        {
            OpenVerbinding();
            var transactie = Verbinding.BeginTransaction();

            try
            {
                regels.ForEach(r => Verbinding.ExecuteNonQuery(r));
                transactie.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                transactie.Rollback();
            }
            finally
            {
                transactie.Dispose();
            }
        }

        public void Dispose()
        {
            SluitVerbinding();
        }
    }
}
