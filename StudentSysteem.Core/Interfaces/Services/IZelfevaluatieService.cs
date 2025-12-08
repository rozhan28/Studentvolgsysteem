using StudentVolgSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfevaluatieService
    {
        void Add(ZelfEvaluatie evaluatie);
        List<ZelfEvaluatie> HaalOpVoorStudent(int studentId);
    }
}
