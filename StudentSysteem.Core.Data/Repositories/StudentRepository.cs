using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

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

            List<string> insertQueries = [@"INSERT OR IGNORE INTO Student(student_id, naam, email, studentnummer, klas) 
                                        VALUES(NULL, 'MockData2', NULL, NULL, NULL)"];
            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}