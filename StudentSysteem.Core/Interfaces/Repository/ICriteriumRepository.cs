using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface ICriteriumRepository
    {
        public List<Criterium> HaalCriteriaOpVoorNiveau(Niveauaanduiding niveau);
        public List<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId, Niveauaanduiding niveau);

        public void SlaGeselecteerdeCriteriaOp(int feedbackId, IEnumerable<Criterium> geselecteerdeCriteria,
            Niveauaanduiding niveau);
    }
}