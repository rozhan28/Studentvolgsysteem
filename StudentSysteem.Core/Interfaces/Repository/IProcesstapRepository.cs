using StudentSysteem.Core.Models;
using System.Collections.Generic;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IProcesstapRepository
    {
        IEnumerable<Processtap> HaalProcesstappenOpVoorProces(int procesId);
    }
}
