using StudentVolgSysteem.Core.Models;

namespace StudentVolgSysteem.Core.Services
{
    public class MockSelfReflectionService : ISelfReflectionService
    {
        private static readonly List<SelfReflection> _reflections = new();

        public void Add(SelfReflection reflection)
        {
            if (string.IsNullOrWhiteSpace(reflection.Leeruitkomst))
                throw new ArgumentException("Leeruitkomst mag niet leeg zijn.");

            if (string.IsNullOrWhiteSpace(reflection.PrestatieNiveau))
                throw new ArgumentException("Selecteer een prestatieniveau.");

            _reflections.Add(reflection);
        }

        public List<SelfReflection> GetByStudent(int studentId)
        {
            return _reflections.Where(x => x.StudentId == studentId).ToList();
        }
    }
}


