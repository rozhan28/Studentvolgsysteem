using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IClustherRepository
    {
        public void MaakTabel(string sqlOpdracht);
        public void VoegMeerdereInMetTransactie(List<string> regels);
    }
}
