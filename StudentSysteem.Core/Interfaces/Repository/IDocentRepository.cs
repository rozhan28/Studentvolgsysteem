using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IDocentRepository
    {
        public Docent? HaalOp();
        public List<Docent> HaalAlleDocentenOp();
    }
}