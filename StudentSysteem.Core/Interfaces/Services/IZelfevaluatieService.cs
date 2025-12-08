using StudentVolgSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfevaluatieService
    {
        void Add(Zelfevaluatie evaluatie);
        List<Zelfevaluatie> HaalOpVoorStudent(int studentId);
    }
}
