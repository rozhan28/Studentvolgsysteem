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
            foreach (Toelichting toelichting in toelichtingen)
            {
                if (string.IsNullOrWhiteSpace(toelichting.Tekst))
                    throw new ArgumentException("Toelichting mag niet leeg zijn.", nameof(toelichting));
            }

            if (studentId <= 0)
                throw new ArgumentException("StudentId moet groter zijn dan 0.", nameof(studentId));

            _feedbackRepository.VoegToelichtingenToe(toelichtingen, studentId);
        }
        
        public IEnumerable<Feedback> HaalFeedbackOp(Datapunt datapunt, int studentId)
        {
            return _feedbackRepository.HaalFeedbackOp(datapunt, studentId);
        }
    }
}

