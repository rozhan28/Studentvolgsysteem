using System.Collections.Generic;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IToelichtingService
    {
        Toelichting MaakNieuweToelichting();
        List<string> GetBeschikbareOpties(IEnumerable<Toelichting> huidigeToelichtingen);
        int TotaleOptiesCount { get; }
    }
}