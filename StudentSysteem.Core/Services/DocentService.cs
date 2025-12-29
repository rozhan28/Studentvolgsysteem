using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class DocentService : IDocentService
{
    private readonly IDocentRepository _docentRepository;
    
    public DocentService(IDocentRepository docentRepo)
    {
        _docentRepository = docentRepo;
    }

    public Docent? HaalOp()
    {
        return _docentRepository.HaalOp();
    }

    public List<Docent> HaalAlleDocentenOp()
    {
        List<Docent> docenten = _docentRepository.HaalAlleDocentenOp();
        return docenten;
    }
    
    public Docent LoginDocent()
    {
        Docent docent = _docentRepository.HaalOp();
        
        if (docent != null)
        {
            docent.Rol = Role.Docent;
        }
        return docent;
    }
}