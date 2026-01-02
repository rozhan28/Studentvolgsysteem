using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IProcesRepository
    {
        public Proces? HaalOp(int procesId);
        public IEnumerable<Proces> HaalAlleProcessenOp();
    }
}
