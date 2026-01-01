using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IZelfEvaluatieService
    {
        int VoegToe(ZelfEvaluatie evaluatie);
        IEnumerable<ZelfEvaluatie> HaalOpVoorStudent(int studentId);
    }
}