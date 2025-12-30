using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface IStudentService
{
    public Student? HaalOp();
    public IEnumerable<Student> HaalAlleStudentenOp();
    public Student? LoginStudent();
}