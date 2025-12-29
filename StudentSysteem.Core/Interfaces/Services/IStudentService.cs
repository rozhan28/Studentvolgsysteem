using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface IStudentService
{
    public Student? HaalOp();
    public List<Student> HaalAlleStudentenOp();
    public Student? LoginStudent();
}