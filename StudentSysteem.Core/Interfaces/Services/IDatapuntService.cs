
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IDatapuntService
    {
        IEnumerable<Datapunt> HaalAlleDatapuntenOp();
    }
}
