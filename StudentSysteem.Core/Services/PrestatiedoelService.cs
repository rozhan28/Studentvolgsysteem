using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class PrestatiedoelService : IPrestatiedoelService
    {
        private readonly IPrestatiedoelRepository _repository;

        public PrestatiedoelService(IPrestatiedoelRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Prestatiedoel> HaalPrestatiedoelenOp()
        {
            return _repository.HaalAllePrestatiedoelenOp();
        }
    }
}
