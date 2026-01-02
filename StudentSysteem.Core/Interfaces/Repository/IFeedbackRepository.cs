using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        void VoegToelichtingenToe(List<Toelichting> toelichtingen, int studentId);
        List<Feedback> HaalFeedbackOp(Datapunt datapunt, int studentId);
    }
}
