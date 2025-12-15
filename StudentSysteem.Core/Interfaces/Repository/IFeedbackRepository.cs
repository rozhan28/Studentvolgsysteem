namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        void VoegToelichtingToe(string toelichting, int studentId);
    }
}
