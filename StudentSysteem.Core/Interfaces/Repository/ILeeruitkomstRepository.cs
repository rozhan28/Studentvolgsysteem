using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface ILeeruitkomstRepository
    {
        public Leeruitkomst? HaalOp();
        public List<Leeruitkomst> HaalAlleLeeruitkomstenOp();
    }
}
