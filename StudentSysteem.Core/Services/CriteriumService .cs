using System.Collections.ObjectModel;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class CriteriumService : ICriteriumService
    {
        private readonly ICriteriumRepository _criteriumRepository;

        public CriteriumService(ICriteriumRepository criteriumRepository)
        {
            _criteriumRepository = criteriumRepository;
        }

        public IEnumerable<Criterium> HaalCriteriaOpVoorPrestatiedoel(int prestatiedoelId)
        {
            return _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId);
        }
        
        public void SlaGeselecteerdeCriteriaOp(int feedbackId, IEnumerable<Criterium> geselecteerdeCriteria)
        {
            _criteriumRepository.SlaGeselecteerdeCriteriaOp(feedbackId, geselecteerdeCriteria);
        }
    }
}