using StudentSysteem.Core.Models;
using System.Collections.Generic;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        void VoegToelichtingenToe(List<Toelichting> toelichtingen, int studentId);
    }
}

