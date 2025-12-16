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

        public void SlaToelichtingOp(int feedbackId, string toelichting)
        {
            if (feedbackId <= 0)
                throw new ArgumentException("FeedbackId moet groter zijn dan 0.", nameof(feedbackId));

            if (string.IsNullOrWhiteSpace(toelichting))
                throw new ArgumentException("Toelichting mag niet leeg zijn.", nameof(toelichting));

            _feedbackRepository.VoegToelichtingToe(feedbackId, toelichting);
        }
    }
}
