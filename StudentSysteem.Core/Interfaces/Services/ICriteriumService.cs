using StudentSysteem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface ICriteriumService
    {
        public List<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId, Niveauaanduiding niveau);
        List<Criterium> HaalOpNiveauCriteriaOp();
        List<Criterium> HaalBovenNiveauCriteriaOp();
    }

}
