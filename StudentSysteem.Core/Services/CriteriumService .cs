using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class CriteriumService : ICriteriumService
    {
        private readonly ICriteriumRepository _repo;

        public CriteriumService(ICriteriumRepository repo)
        {
            _repo = repo;
        }

        public List<Criterium> HaalOpNiveauCriteriaOp()
            => _repo.HaalCriteriaOpVoorNiveau("OpNiveau");

        public List<Criterium> HaalBovenNiveauCriteriaOp()
            => _repo.HaalCriteriaOpVoorNiveau("BovenNiveau");
    }
}
