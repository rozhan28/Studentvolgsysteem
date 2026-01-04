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

        public List<string> GetBeschikbareCriteria(IEnumerable<Toelichting> huidigeToelichtingen, int prestatiedoelId)
        {
            // Criteria ophalen voor beide niveaus
            List<Criterium> opNiveau = _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId);
            List<Criterium> bovenNiveau = new();

            // Lijst maken van alle mogelijke criteria
            List<string> alleTitels = opNiveau
                .Concat(bovenNiveau)
                .Select(delegate (Criterium c) { return c.Beschrijving; })
                .ToList();

            // 'Algemeen' toevoegen aan het begin
            alleTitels.Insert(0, "Algemeen");

            // Gekozen criteria in een HashSet
            HashSet<string> gekozenCriteria = new HashSet<string>();
            foreach (Toelichting t in huidigeToelichtingen ?? Enumerable.Empty<Toelichting>())
            {
                if (!string.IsNullOrEmpty(t.GeselecteerdeOptie) && t.GeselecteerdeOptie != "Toelichting gekoppeld aan...")
                {
                    gekozenCriteria.Add(t.GeselecteerdeOptie);
                }
            }

            // Filter criteria die nog niet gekozen zijn
            List<string> beschikbareTitels = new List<string>();
            foreach (string titel in alleTitels)
            {
                if (!gekozenCriteria.Contains(titel))
                {
                    beschikbareTitels.Add(titel);
                }
            }

            return beschikbareTitels;
        }

        public void KoppelGekozenOptie(Toelichting toelichting, string gekozenTitel, int prestatiedoelId)
        {
            if (toelichting == null || string.IsNullOrEmpty(gekozenTitel)) return;

            toelichting.GeselecteerdeOptie = gekozenTitel;

            if (gekozenTitel == "Algemeen")
            {
                toelichting.Niveau = "Algemeen";
            }
            else
            {
                // Criterium opzoeken om het niveau te achterhalen
                List<Criterium> criteria = _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId)
                    .Concat(_criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId))
                    .ToList();

                Criterium gevondenCriterium = criteria.FirstOrDefault(delegate (Criterium c) { return c.Beschrijving == gekozenTitel; });
                toelichting.Niveau = gevondenCriterium != null ? gevondenCriterium.Niveau.ToString() : "";
            }
        }

        public int BerekenMaxToelichtingen(int prestatiedoelId)
        {
            int aantalOpNiveau = _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId).Count;
            int aantalBovenNiveau = _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(prestatiedoelId).Count;
            
            // De som van de criteria plus 1 voor 'Algemeen'
            return aantalOpNiveau + aantalBovenNiveau + 1;
        }
    }
}