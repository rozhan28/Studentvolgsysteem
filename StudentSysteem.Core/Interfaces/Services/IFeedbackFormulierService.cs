namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IFeedbackFormulierService
    {
        void SlaToelichtingOp(string toelichting, int studentId = 1);
    }
}