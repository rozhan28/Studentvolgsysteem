using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class BeoordelingStructuurService : IBeoordelingStructuurService
{
    private readonly IProcesRepository _procesRepository;
    private readonly IProcesstapRepository _processtapRepository;
    private readonly IVaardigheidRepository _vaardigheidRepository;
    private readonly IPrestatiedoelRepository _prestatiedoelRepository;

    public BeoordelingStructuurService(
        IProcesRepository procesRepository,
        IProcesstapRepository processtapRepository,
        IVaardigheidRepository vaardigheidRepository,
        IPrestatiedoelRepository prestatiedoelRepository)
    {
        _procesRepository = procesRepository;
        _processtapRepository =  processtapRepository;
        _vaardigheidRepository = vaardigheidRepository;
        _prestatiedoelRepository = prestatiedoelRepository;
    }
    
    public IEnumerable<BeoordelingStructuur> HaalVolledigeStructuurOp()
    {
        List<BeoordelingStructuur> resultaat = new List<BeoordelingStructuur>();
        
        IEnumerable<Proces> processen = _procesRepository.HaalAlleProcessenOp();
        IEnumerable<Vaardigheid> vaardigheden = _vaardigheidRepository.HaalAlleVaardighedenOp();
        IEnumerable<Prestatiedoel> prestatiedoelen = _prestatiedoelRepository.HaalAllePrestatiedoelenOp();

        foreach (Proces proces in processen)
        {
            // Processtappen opvragen voor specifiek proces
            IEnumerable<Processtap> processtappen = _processtapRepository.HaalProcesstappenOpVoorProces(proces.Id);

            foreach (Processtap processtap in processtappen)
            {
                // Vaardigheden opvragen voor specifiek processtap
                IEnumerable<Vaardigheid> vaardighedenVoorStap = vaardigheden.Where(v => v.ProcesstapId == processtap.Id);

                foreach (Vaardigheid v in vaardighedenVoorStap)
                {
                    // Prestatiedoel opvragen
                    Prestatiedoel prestatiedoel = prestatiedoelen.FirstOrDefault(d => d.Id == v.PrestatiedoelId);

                    if (prestatiedoel != null)
                    {
                        // Maak BeoordelingStructuur-pakketje
                        resultaat.Add(new BeoordelingStructuur(proces, processtap, v, prestatiedoel));
                    }
                }
            }
        }
        return resultaat;
    }
}