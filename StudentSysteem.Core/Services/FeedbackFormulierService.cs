using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.Core.Services
{
    public class FeedbackFormulierService : IFeedbackFormulierService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICriteriumRepository _criteriumRepository;

        public FeedbackFormulierService(IFeedbackRepository feedbackRepository, ICriteriumRepository criteriumRepository)
        {
            _feedbackRepository = feedbackRepository;
            _criteriumRepository = criteriumRepository;
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

