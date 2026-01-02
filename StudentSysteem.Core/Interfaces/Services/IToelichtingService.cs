using StudentSysteem.Core.Models;
using StudentSysteem.Core.Services;
using StudentSysteem.Core.Models;
using System.Collections.Generic;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IToelichtingService
    {
        Toelichting MaakNieuweToelichting();
        List<ToelichtingOptie> GetBeschikbareOpties(IEnumerable<Toelichting> huidigeToelichtingen, int prestatiedoelId);
        int TotaleOptiesCount { get; }
    }
}
