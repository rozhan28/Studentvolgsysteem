using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class ProcesstapService : IProcesstapService
    {
        private readonly IProcesstapRepository _processtapRepository;
        public ProcesstapService(IProcesstapRepository processtapRepository)
        {
            _processtapRepository = processtapRepository;
        }
        public IEnumerable<Processtap> HaalProcesstappenOpVoorProces(int procesId)
        {
            return _processtapRepository.HaalProcesstappenOpVoorProces(procesId);
        }
    }
}
