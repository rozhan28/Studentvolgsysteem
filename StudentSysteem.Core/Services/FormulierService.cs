using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class FormulierService : IFormulierService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FormulierService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId, int prestatiedoelId, int feedbackgeverId)
        {
            if (studentId <= 0)
                throw new ArgumentException("StudentId moet groter zijn dan 0.");
            
            if (prestatiedoelId <= 0)
                throw new ArgumentException("PrestatiedoelId is vereist.");
            
            if (feedbackgeverId <= 0)
                throw new ArgumentException("FeedbackgeverId is vereist.");

            List<Toelichting> gevuldeToelichtingen = toelichtingen
                .Where(t => !string.IsNullOrWhiteSpace(t.Tekst))
                .ToList();

            if (gevuldeToelichtingen.Any())
                _feedbackRepository.VoegToelichtingenToe(gevuldeToelichtingen, studentId, prestatiedoelId, feedbackgeverId);
        }
    }
}