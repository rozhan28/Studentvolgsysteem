using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

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

        public void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId = 1)
        {
            if (studentId <= 0)
                throw new ArgumentException("StudentId moet groter zijn dan 0.");

            var gevuldeToelichtingen = toelichtingen
                .Where(t => !string.IsNullOrWhiteSpace(t.Tekst))
                .ToList();

            if (gevuldeToelichtingen.Any())
                _feedbackRepository.VoegToelichtingenToe(gevuldeToelichtingen, studentId);
        }
    }
}
