using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface ICriteriumRepository
    {
        public List<Criterium> HaalCriteriaOpVoorNiveau(Niveauaanduiding niveauaanduiding);
        public List<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId, Niveauaanduiding niveauaanduiding);

        public void SlaGeselecteerdeCriteriaOp(int feedbackId, IEnumerable<Criterium> geselecteerdeCriteria,Niveauaanduiding niveau);
    }
}