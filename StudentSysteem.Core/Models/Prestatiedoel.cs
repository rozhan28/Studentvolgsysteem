using System.Collections.Generic;

namespace StudentSysteem.Core.Models
{
    public class Prestatiedoel
    {
        public int Id { get; set; }
        public string Niveau { get; set; } = "";
        public string Beschrijving { get; set; } = "";

        public int? CriteriumId { get; set; }
        public string AiAssessmentScale { get; set; }

        public List<Criterium> Criteria { get; set; } = new();
    }
}
