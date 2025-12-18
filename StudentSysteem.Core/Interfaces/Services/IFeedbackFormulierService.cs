using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IFeedbackFormulierService
    {
        void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId = 1);
    }
}