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

        private void ValideerFeedback(List<Feedback> feedbackLijst)
        {
            foreach (Feedback feedback in feedbackLijst)
            {
                if (feedback.StudentId <= 0)
                    throw new ArgumentException("StudentId moet groter zijn dan 0.");

                if (feedback.VaardigheidId <= 0)
                    throw new ArgumentException("PrestatiedoelId is vereist.");

                if (feedback.FeedbackGeverId <= 0 && feedback.DocentId <= 0)
                    throw new ArgumentException("FeedbackgeverId of DocentId is vereist.");

                foreach (Toelichting toelichting in feedback.Toelichtingen)
                {
                    if (string.IsNullOrWhiteSpace(toelichting.Tekst))
                        throw new ArgumentException("Toelichting moet tekst bevatten.");
                }
            }
        }

        public void SlaFeedbackOp(List<Feedback> feedbackLijst)
        {
            // ValideerFeedback gooit nu een exception als iets fout is
            ValideerFeedback(feedbackLijst);

            // Alleen hier komt het bij de repository als alles geldig is
            _feedbackRepository.VoegFeedbackToe(feedbackLijst);
        }

    }
}