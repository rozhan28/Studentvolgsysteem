using StudentVolgSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfReflectieService
    {
        void Add(ZelfReflectie reflectie);
        List<ZelfReflectie> HaalOpVoorStudent(int studentId);
    }
}
