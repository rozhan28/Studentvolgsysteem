using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class LeeruitkomstService: ILeeruitkomstService
{
    private readonly ILeeruitkomstRepository _repository;

    public LeeruitkomstService(ILeeruitkomstRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Leeruitkomst> HaalAlleLeeruitkomstenOp()
    {
        return _repository.HaalAlleLeeruitkomstenOp();
    }
}