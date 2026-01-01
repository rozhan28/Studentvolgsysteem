using StudentSysteem.Core.Models;
using System.Collections.Generic;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        // Oude methoden
        int MaakFeedbackAan(string niveau);
        void VoegToelichtingToe(int feedbackId, string toelichting);

        // Nieuwe methoden
        void VoegToelichtingenToe(List<Toelichting> toelichtingen, int studentId);
    }
}
