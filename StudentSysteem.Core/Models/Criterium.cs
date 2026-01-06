namespace StudentSysteem.Core.Models
{
    public class Criterium
    {
        public int Id { get; set; }
        public string Beschrijving { get; set; }
        public Niveauaanduiding Niveau { get; set; }
        
        public bool IsGeselecteerd { get; set; } = false;

        public Criterium(int id, string beschrijving, Niveauaanduiding niveau)
        {
            Id = id;
            Beschrijving = beschrijving;
            Niveau = niveau;
        }
    }
}