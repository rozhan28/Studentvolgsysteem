using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class VaardigheidService : IVaardigheidRepository
    {
        private readonly IVaardigheidRepository _vaardigheidrepository;



        public List<Vaardigheid> HaalAlleVaardighedenOp()
        {
            return _vaardigheidrepository.HaalAlleVaardighedenOp();
        }
    }
}
