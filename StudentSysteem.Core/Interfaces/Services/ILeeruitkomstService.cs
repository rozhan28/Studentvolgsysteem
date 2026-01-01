using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface ILeeruitkomstService
{
    public Leeruitkomst? HaalOp();
    public IEnumerable<Leeruitkomst> HaalAlleLeeruitkomstenOp();
}