using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.Core.Services
{
    public class FeedbackFormulierService : IFeedbackFormulierService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackFormulierService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public void SlaToelichtingOp(string toelichting, int studentId = 1)
        {
            if (string.IsNullOrWhiteSpace(toelichting))
                throw new ArgumentException("Toelichting mag niet leeg zijn.", nameof(toelichting));

            if (studentId <= 0)
                throw new ArgumentException("StudentId moet groter zijn dan 0.", nameof(studentId));

            _feedbackRepository.VoegToelichtingToe(toelichting, studentId);
        }
    }
}

