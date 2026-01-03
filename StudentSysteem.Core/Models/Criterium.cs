namespace StudentSysteem.Core.Models
{
    public class Criterium
    {
        public int Id { get; set; }
        public string Beschrijving { get; set; } = "";
        public string Niveau { get; set; } = "";

        public Criterium(int id, string beschrijving, string niveau)
        {
            Id = id;
            Beschrijving = beschrijving;
            Niveau = niveau;
        }
    }
}