namespace StudentSysteem.Core.Models
{
    public class Prestatiedoel
    {
        public int Id { get; set; }
        public string Niveau { get; set; } = "";
        public string Beschrijving { get; set; } = "";

        // Optioneel uit develop-test
        public int? CriteriumId { get; set; }
        public string AiAssessmentScale { get; set; }

        // Lijst van gekoppelde criteria
        public List<Criterium> Criteria { get; set; } = new();
    }
}
