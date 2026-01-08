using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using System.Data;
using System.Diagnostics;

namespace StudentSysteem.Core.Data
{
    public abstract class DatabaseVerbinding : IDisposable
    {
        protected SqliteConnection Verbinding { get; }
        private string databaseBestandsnaam;

        public DatabaseVerbinding(DbConnectieHelper dbConnectieHelper)
        {
            databaseBestandsnaam = dbConnectieHelper.ConnectieStringWaarde("StepwiseDb");
            
            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string schoneBaseDir = baseDir.Trim();
            string schoneBestandsnaam = databaseBestandsnaam.Trim();
            string dbPath = schoneBaseDir.TrimEnd(Path.DirectorySeparatorChar)
                            + Path.DirectorySeparatorChar
                            + schoneBestandsnaam;
            string dbConnection = $"Data Source={dbPath}; Foreign Keys=True";
            Verbinding = new SqliteConnection(dbConnection);


            Debug.WriteLine("DB PATH: " + dbPath);
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
            using (SqliteCommand command = Verbinding.CreateCommand())
            {
                command.CommandText = sqlOpdracht;
                command.ExecuteNonQuery();
            }
        }
        
        public void VoegMeerdereInMetTransactie(List<string> regels)
        {
            OpenVerbinding();
            SqliteTransaction transactie = Verbinding.BeginTransaction();

            try
            {
                using SqliteCommand cmd = Verbinding.CreateCommand();
                cmd.Transaction = transactie; 
        
                foreach (String sql in regels)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
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
        
        public void VerwijderInhoud(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name cannot be empty.", nameof(tableName));

            OpenVerbinding();
            try
            {
                using SqliteCommand command = Verbinding.CreateCommand();
                command.CommandText = $"DELETE FROM {tableName};";
                command.ExecuteNonQuery();
            }
            finally
            {
                SluitVerbinding();
            }
        }

        protected IEnumerable<T> VoerSelectUit<T>(
    string sql,
    Func<IDataReader, T> mapFunc,
    Dictionary<string, object>? parameters = null)
        {
            OpenVerbinding();

            try
            {
                using SqliteCommand command = Verbinding.CreateCommand();
                command.CommandText = sql;

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                using SqliteDataReader reader = command.ExecuteReader();
                List<T> resultaten = new();

                while (reader.Read())
                {
                    resultaten.Add(mapFunc(reader));
                }

                return resultaten;
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