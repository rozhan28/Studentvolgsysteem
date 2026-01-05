using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface ICriteriumRepository
    {
        public List<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId);
        public void SlaGeselecteerdeCriteriaOp(int feedbackId, IEnumerable<Criterium> geselecteerdeCriteria);
    }
}