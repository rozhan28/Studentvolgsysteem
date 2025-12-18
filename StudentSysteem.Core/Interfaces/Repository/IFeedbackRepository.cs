namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        void VoegToelichtingenToe(string toelichting, int studentId);
    }
}
