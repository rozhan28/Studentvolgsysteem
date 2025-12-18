using System.Collections.Generic;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IToelichtingService
    {
        IReadOnlyList<Criterium> GetCriteriumLijst();
        IReadOnlyList<string> GetBeschikbareOpties(IEnumerable<Toelichting> bestaande);
    }
}