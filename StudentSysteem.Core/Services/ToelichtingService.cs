using System.Collections.Generic;
using System.Linq;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services
{
    public class ToelichtingService
    {
        private readonly List<string> _beschikbareOpties = new()
        {
            "Algemeen",
            "Criteria 1",
            "Criteria 2",
            "Criteria 3"
        };

        public Toelichting MaakNieuweToelichting()
        {
            return new Toelichting();
        }

        public List<string> GetBeschikbareOpties(IEnumerable<Toelichting> huidigeToelichtingen)
        {
            var gebruikte = new HashSet<string>();
            foreach (var t in huidigeToelichtingen ?? Enumerable.Empty<Toelichting>())
            {
                if (!string.IsNullOrEmpty(t.GeselecteerdeOptie) && t.GeselecteerdeOptie != "Toelichting gekoppeld aan...")
                    gebruikte.Add(t.GeselecteerdeOptie);
            }
            return _beschikbareOpties.Where(x => !gebruikte.Contains(x)).ToList();
        }

        public int TotaleOptiesCount => _beschikbareOpties.Count;
    }
}