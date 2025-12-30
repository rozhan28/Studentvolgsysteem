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

        public void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId = 1)
        {
            if (studentId <= 0)
                throw new ArgumentException("StudentId moet groter zijn dan 0.");
            
            List<Toelichting> gevuldeToelichtingen = toelichtingen
                .Where(t => !string.IsNullOrWhiteSpace(t.Tekst))
                .ToList();
            
            if (gevuldeToelichtingen.Count > 0)
            {
                _feedbackRepository.VoegToelichtingenToe(gevuldeToelichtingen, studentId);
            }
        }
    }
}

