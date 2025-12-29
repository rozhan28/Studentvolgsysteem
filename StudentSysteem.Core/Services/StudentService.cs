using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    
    public StudentService(IStudentRepository studentRepo)
    {
        _studentRepository = studentRepo;
    }

    public Student? HaalOp()
    {
        return _studentRepository.HaalOp();
    }

    public List<Student> HaalAlleStudentenOp()
    {
        List<Student> studenten = _studentRepository.HaalAlleStudentenOp();
        return studenten;
    }
    
    public Student LoginStudent()
    {
        Student student = _studentRepository.HaalOp();
        
        if (student != null)
        {
            student.Rol = Role.Student;
        }
        return student;
    }
}