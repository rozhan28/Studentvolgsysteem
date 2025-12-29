using Microsoft.Data.Sqlite;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class StudentRepository : DatabaseVerbinding, IStudentRepository
    {
        public StudentRepository(DbConnectieHelper dbConnectieHelper) : base(dbConnectieHelper)
        {
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Student (
                    [student_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(50),
                    [email] VARCHAR(50),
                    [studentnummer] VARCHAR(50),
                    [klas] VARCHAR(5))");

            List<string> insertQueries = [@"INSERT OR REPLACE INTO Student(student_id, naam, email, studentnummer, klas) 
                                        VALUES(1, 'Sanne', 'sanne.abrahamse@windesheim.nl', '1199564', 'ICTOOSDDa')"];
            VoegMeerdereInMetTransactie(insertQueries);
        }

        public Student? HaalOp()
        {
            List<Student> alleStudenten = HaalAlleStudentenOp();
    
            // We retourneren even de eerste student uit de lijst, omdat er nog maar 1 is.
            // Indien er meer studenten zijn en we hebben een inlogfunctie,
            // moeten we hier een WHERE email = ingevulde email check doen.
            return alleStudenten.FirstOrDefault();
        }

        public List<Student> HaalAlleStudentenOp()
        {
            List<Student> studenten = new();
            studenten.Clear();
            string selectQuery = "SELECT student_id, naam, email, studentnummer, klas FROM Student";
            OpenVerbinding();

            using (SqliteCommand command = new(selectQuery, Verbinding))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int StudentId = reader.GetInt32(0);
                    string StudentNaam = reader.GetString(1);
                    string StudentEmail = reader.GetString(2);
                    string StudentNummer = reader.GetString(3);
                    string Klas = reader.GetString(4);
                    studenten.Add(new(StudentId, StudentNaam, StudentEmail, StudentNummer, Klas));
                }
            }
            SluitVerbinding();
            return studenten;
        }
    }
}