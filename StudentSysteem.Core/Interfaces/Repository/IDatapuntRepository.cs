using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IDatapuntRepository
    {
        public List<Datapunt> HaalAlleDatapuntenOp();
    }
}
