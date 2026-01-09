using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class DatapuntService : IDatapuntService
{
    private readonly IDatapuntRepository _datapuntRepository;

    public DatapuntService(IDatapuntRepository repository)
    {
        _datapuntRepository = repository;
    }

    public IEnumerable<Datapunt> HaalAlleDatapuntenOpVanStudent(int StudentId)
    {
        return _datapuntRepository.HaalAlleDatapuntenOpVanStudent(StudentId);
    }
}