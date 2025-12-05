using System;
using System.Collections.Generic;
using System.Text;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.Core.Services
{
    public class FeedbackFormulierService : IFeedbackFormulierService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackFormulierService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public void SlaToelichtingOp(string toelichting, int studentId = 1)
        {
            _feedbackRepository.VoegToelichtingToe(toelichting, studentId);
        }
    }
}

