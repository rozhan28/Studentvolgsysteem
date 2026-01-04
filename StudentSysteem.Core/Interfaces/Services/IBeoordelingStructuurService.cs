using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface IBeoordelingStructuurService
{
    public IEnumerable<BeoordelingStructuur> HaalVolledigeStructuurOp();
}