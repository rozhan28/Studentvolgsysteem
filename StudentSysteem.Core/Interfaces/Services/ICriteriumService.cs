using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface ICriteriumService
    {
        public IEnumerable<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId);
        public void SlaGeselecteerdeCriteriaOp(int feedbackId, IEnumerable<Criterium> geselecteerdeCriteria);
    }
}