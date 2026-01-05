using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IFormulierService
    {
        void SlaFeedbackOp(List<Feedback> feedback);
        bool ValideerFeedback(List<Feedback> feedbackLijst);
    }
}