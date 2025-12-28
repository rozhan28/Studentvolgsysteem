using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IFeedbackFormulierService
    {
        void SlaToelichtingOp(int feedbackId, string toelichting);

        void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId = 1);
    }
}
