using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class ZelfEvaluatieService : IZelfEvaluatieService
    {
        private static readonly List<ZelfEvaluatie> _reflecties = new();

        public int Add(ZelfEvaluatie reflectie)
        {
            if (string.IsNullOrWhiteSpace(reflectie.PrestatieNiveau))
                throw new ArgumentException("Selecteer een prestatieniveau.");

            _reflecties.Add(reflectie);

            return _reflecties.Count;
        }

        public List<ZelfEvaluatie> HaalOpVoorStudent(int studentId)
        {
            return _reflecties
                .Where(x => x.StudentId == studentId)
                .ToList();
        }
    }
}
