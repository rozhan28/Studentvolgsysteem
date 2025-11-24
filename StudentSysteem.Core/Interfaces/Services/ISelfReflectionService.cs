using StudentVolgSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface ISelfReflectionService
    {
        void Add(SelfReflection reflection);
        List<SelfReflection> GetByStudent(int studentId);
    }
}


