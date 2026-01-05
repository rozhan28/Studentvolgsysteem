using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IProcesstapService
    {
        public IEnumerable<Processtap> HaalProcesstappenOpVoorProces(int procesId);
    }
}
