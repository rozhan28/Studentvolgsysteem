using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IStudentRepository
    {
        public Student? HaalOp();
        public List<Student> HaalAlleStudentenOp();
    }
}