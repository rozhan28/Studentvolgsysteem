using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IVaardigheidRepository
    {
        public List<Vaardigheid> HaalAlleVaardighedenOp();
    }
}