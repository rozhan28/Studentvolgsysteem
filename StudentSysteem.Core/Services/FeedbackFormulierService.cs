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

        // Vanuit feature/US3_Niveau
        public void SlaToelichtingOp(int feedbackId, string toelichting)
        {
            if (feedbackId <= 0)
                throw new ArgumentException("FeedbackId moet groter zijn dan 0.", nameof(feedbackId));

            if (string.IsNullOrWhiteSpace(toelichting))
                throw new ArgumentException("Toelichting mag niet leeg zijn.", nameof(toelichting));

            _feedbackRepository.VoegToelichtingToe(feedbackId, toelichting);
        }

        // Vanuit merge/develop-US3
        public void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId = 1)
        {
            if (toelichtingen == null || toelichtingen.Count == 0)
                throw new ArgumentException("Er moet minimaal één toelichting zijn.", nameof(toelichtingen));

            foreach (Toelichting toelichting in toelichtingen)
            {
                if (string.IsNullOrWhiteSpace(toelichting.Tekst))
                    throw new ArgumentException("Toelichting mag niet leeg zijn.", nameof(toelichting));
            }

            if (studentId <= 0)
                throw new ArgumentException("StudentId moet groter zijn dan 0.", nameof(studentId));

            _feedbackRepository.VoegToelichtingenToe(toelichtingen, studentId);
        }
    }
}
