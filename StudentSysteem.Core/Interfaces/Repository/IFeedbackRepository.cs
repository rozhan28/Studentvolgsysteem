using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        void MaakTabel(string sqlOpdracht);
        void VoegMeerdereInMetTransactie(List<string> regels);

        void VoegToelichtingToe(string toelichting, int studentId);
    }
}
