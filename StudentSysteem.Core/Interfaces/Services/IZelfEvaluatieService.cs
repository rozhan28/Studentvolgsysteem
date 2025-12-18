using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfEvaluatieService
    {
        int Add(ZelfEvaluatie evaluatie);
        List<ZelfEvaluatie> HaalOpVoorStudent(int studentId);
    }
}