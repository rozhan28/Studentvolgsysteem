using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class VaardigheidService : IVaardigheidService
    {
        private readonly IVaardigheidRepository _vaardigheidRepository;

        public VaardigheidService(IVaardigheidRepository vaardigheidRepository)
        {
            _vaardigheidRepository = vaardigheidRepository;
        }
        
        public IEnumerable<Vaardigheid> HaalAlleVaardighedenOp()
        {
            return _vaardigheidRepository.HaalAlleVaardighedenOp();
        }
    }
}