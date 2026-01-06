using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        void VoegFeedbackToe(List<Feedback> feedback);
    }

}