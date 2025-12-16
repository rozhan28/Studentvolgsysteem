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

        public List<Criterium> HaalOpNiveauCriteriaOp()
        {
            return _criteriumRepository.HaalCriteriaOpVoorNiveau("Op niveau");
        }

        public List<Criterium> HaalBovenNiveauCriteriaOp()
        {
            return _criteriumRepository.HaalCriteriaOpVoorNiveau("Boven niveau");
        }
    }
}