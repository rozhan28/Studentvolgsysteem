using StudentVolgSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfevaluatieService
    {
        void Add(ZelfReflectie reflectie);
        List<ZelfReflectie> HaalOpVoorStudent(int studentId);
    }
}
