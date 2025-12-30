using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface ILeeruitkomstService
    {
        IEnumerable<Leeruitkomst> HaalAlleLeeruitkomstenOp();
    }
}
