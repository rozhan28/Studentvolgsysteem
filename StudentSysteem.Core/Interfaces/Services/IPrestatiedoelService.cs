using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IPrestatiedoelService
    {
        IEnumerable<Prestatiedoel> HaalPrestatiedoelenOp();
    }
}