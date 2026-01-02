using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IProcesstapRepository
    {
        public IEnumerable<Processtap> HaalProcesstappenOpVoorProces(int procesId);
    }
}
