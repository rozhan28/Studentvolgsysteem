using StudentVolgSysteem.Core.Models;

namespace StudentVolgSysteem.Core.Services
{
    public interface ISelfReflectionService
    {
        void Add(SelfReflection reflection);
        List<SelfReflection> GetByStudent(int studentId);
    }
}


