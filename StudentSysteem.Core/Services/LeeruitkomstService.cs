using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class LeeruitkomstService : ILeeruitkomstService
{
    private readonly ILeeruitkomstRepository _leeruitkomstRepository;

    public LeeruitkomstService(ILeeruitkomstRepository leeruitkomstRepository)
    {
        _leeruitkomstRepository = leeruitkomstRepository;
    }
    
    public Leeruitkomst? HaalOp()
    {
        return _leeruitkomstRepository.HaalOp();
    }

    public IEnumerable<Leeruitkomst> HaalAlleLeeruitkomstenOp()
    {
        return _leeruitkomstRepository.HaalAlleLeeruitkomstenOp();
    }
}