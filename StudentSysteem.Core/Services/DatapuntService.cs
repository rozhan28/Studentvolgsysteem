using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class DatapuntService:IDatapuntService
{
    private readonly IDatapuntRepository _repository;

    public DatapuntService(IDatapuntRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Datapunt> HaalAlleDatapuntenOpVanStudent(int StudentId)
    {
        return _repository.HaalAlleDatapuntenOpVanStudent(StudentId);
    }
}