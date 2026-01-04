using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface ICriteriumService
    {
        public List<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId, Niveauaanduiding niveau);
        public List<Criterium> HaalOpNiveauCriteriaOp();
        public List<Criterium> HaalBovenNiveauCriteriaOp();
        public void SlaGeselecteerdeCriteriaOp(int feedbackId, IEnumerable<Criterium> geselecteerdeCriteria);
    }
}