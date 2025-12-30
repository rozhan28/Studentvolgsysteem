using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface ILeeruitkomstRepository
    {
        public List<Leeruitkomst> HaalAlleLeeruitkomstenOp();
    }
}
