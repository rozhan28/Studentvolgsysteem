using StudentVolgSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfEvaluatieService
    {
        void Add(ZelfEvaluatie evaluatie);
        List<ZelfEvaluatie> HaalOpVoorStudent(int studentId);
    }
}