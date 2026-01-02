using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class ProcesService
{
    private readonly IProcesRepository _procesRepository;
    
    public ProcesService(IProcesRepository procesRepository)
    {
        _procesRepository = procesRepository;
    }
    
    public Proces? HaalOp(int procesId)
    {
        return _procesRepository.HaalOp(procesId);
    }
    
    public IEnumerable<Proces> HaalAlleProcessenOp()
    {
        return _procesRepository.HaalAlleProcessenOp();
    }
}