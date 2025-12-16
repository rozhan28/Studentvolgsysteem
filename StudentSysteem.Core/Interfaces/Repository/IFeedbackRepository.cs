namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        int MaakFeedbackAan(string niveau);
        void VoegToelichtingToe(int feedbackId, string toelichting);
    }
}
