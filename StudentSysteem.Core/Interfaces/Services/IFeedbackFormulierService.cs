using StudentSysteem.Core.Models;
using System.Collections.Generic;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IFeedbackFormulierService
    {
        // Oude methode
        void SlaToelichtingOp(int feedbackId, string toelichting);

        // Nieuwe methode
        void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId = 1);
    }
}
