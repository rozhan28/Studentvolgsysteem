using StudentVolgSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfReflectieService
    {
        void Add(ZelfEvaluatie reflectie);
        List<ZelfEvaluatie> HaalOpVoorStudent(int studentId);
    }
}
