using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IPrestatiedoelRepository
    {
        List<Prestatiedoel> HaalAllePrestatiedoelenOp();
        List<Prestatiedoel> HaalAllePrestatiedoelenOpMetCriteria();
    }
}
