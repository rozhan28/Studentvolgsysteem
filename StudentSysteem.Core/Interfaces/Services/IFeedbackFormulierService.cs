using StudentSysteem.Core.Models;
using System.Collections.Generic;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IFeedbackFormulierService
    {
        void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId = 1);
    }
}
