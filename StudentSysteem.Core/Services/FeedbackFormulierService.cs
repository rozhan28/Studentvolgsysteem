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

        // Samengevoegde methode voor één of meerdere toelichtingen
        public void SlaToelichtingenOp(IEnumerable<Toelichting> toelichtingen, int feedbackId = 0, int studentId = 1)
        {
            if (feedbackId < 0)
                throw new ArgumentException("FeedbackId moet 0 of groter zijn.", nameof(feedbackId));

            if (studentId <= 0)
                throw new ArgumentException("StudentId moet groter zijn dan 0.", nameof(studentId));

            // Converteer enkelvoudige toelichting naar lijst als nodig
            var gevuldeToelichtingen = toelichtingen
                .Where(t => t != null && !string.IsNullOrWhiteSpace(t.Tekst))
                .ToList();

            if (gevuldeToelichtingen.Count == 0)
                return;

            // Als feedbackId is meegegeven, gebruik VoegToelichtingToe voor compatibiliteit
            if (feedbackId > 0 && gevuldeToelichtingen.Count == 1)
            {
                _feedbackRepository.VoegToelichtingToe(feedbackId, gevuldeToelichtingen[0].Tekst);
            }
            else
            {
                _feedbackRepository.VoegToelichtingenToe(gevuldeToelichtingen, studentId);
            }
        }
    }
}

