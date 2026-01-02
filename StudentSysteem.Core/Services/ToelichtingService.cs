using System.Collections.Generic;
using System.Linq;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Services
{
    public class ToelichtingService : IToelichtingService
    {
        private readonly ICriteriumRepository _criteriumRepository;

        public ToelichtingService(ICriteriumRepository criteriumRepository)
        {
            _criteriumRepository = criteriumRepository;
        }

        public Toelichting MaakNieuweToelichting()
        {
            return new Toelichting
            {
                GeselecteerdeOptie = "Toelichting gekoppeld aan...",
                Niveau = ""
            };
        }

        public List<ToelichtingOptie> GetBeschikbareOpties(IEnumerable<Toelichting> huidigeToelichtingen, int prestatiedoelId)
        {
            var alleOpties = _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId, Niveauaanduiding.OpNiveau)
                .Concat(_criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId, Niveauaanduiding.BovenNiveau))
                .Select(c => new ToelichtingOptie
                {
                    Beschrijving = c.Beschrijving,
                    Niveau = c.Niveau
                })
                .ToList();

            alleOpties.Insert(0, new ToelichtingOptie
            {
                Beschrijving = "Algemeen",
                Niveau = "" 
            });

            var gebruikte = new HashSet<string>();
            foreach (var t in huidigeToelichtingen ?? Enumerable.Empty<Toelichting>())
            {
                if (!string.IsNullOrEmpty(t.GeselecteerdeOptie) && t.GeselecteerdeOptie != "Toelichting gekoppeld aan...")
                    gebruikte.Add(t.GeselecteerdeOptie);
            }

            return alleOpties.Where(x => !gebruikte.Contains(x.Beschrijving)).ToList();
        }


        public void KiesOptieVoorToelichting(Toelichting toelichting, ToelichtingOptie optie)
        {
            if (toelichting != null && optie != null)
            {
                toelichting.GeselecteerdeOptie = optie.Beschrijving;
                toelichting.Niveau = optie.Beschrijving == "Algemeen" ? "" : optie.Niveau;
            }
        }

        public int TotaleOptiesCount
        {
            get
            {
                return _criteriumRepository.HaalCriteriaOpVoorNiveau(Niveauaanduiding.OpNiveau).Count +
                       _criteriumRepository.HaalCriteriaOpVoorNiveau(Niveauaanduiding.BovenNiveau).Count + 1;
            }
        }
    }

    public class ToelichtingOptie
    {
        public string Beschrijving { get; set; } = "";
        public string Niveau { get; set; } = "";
        public override string ToString() => string.IsNullOrEmpty(Niveau) ? Beschrijving : $"{Beschrijving} ({Niveau})";
    }
}
