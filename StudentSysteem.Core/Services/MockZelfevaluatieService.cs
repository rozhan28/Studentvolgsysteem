using StudentSysteem.Core.Interfaces.Services;
using StudentVolgSysteem.Core.Models;

namespace StudentVolgSysteem.Core.Services
{
    public class MockZelfevaluatieService : IZelfevaluatieService
    {
        private static readonly List<ZelfReflectie> _reflecties = new();

        public void Add(ZelfReflectie reflectie)
        {
            if (string.IsNullOrWhiteSpace(reflectie.PrestatieNiveau))
                throw new ArgumentException("Selecteer een prestatieniveau.");

            _reflecties.Add(reflectie);
        }

        public List<ZelfReflectie> HaalOpVoorStudent(int studentId)
        {
            return _reflecties.Where(x => x.StudentId == studentId).ToList();
        }
    }
}
