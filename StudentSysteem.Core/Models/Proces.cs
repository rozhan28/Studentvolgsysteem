namespace StudentSysteem.Core.Models
{
    public class Proces
    {
        public int Id { get; set; }
        public string Naam { get; set; }

        public Proces(int id, string naam)
        {
            Id = id;
            Naam = naam;
        }
    }
}
