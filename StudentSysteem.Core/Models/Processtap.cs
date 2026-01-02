namespace StudentSysteem.Core.Models
{
    public class Processtap
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public int ProcesId { get; set; }
        public int CriteriumId { get; set; }

        public Processtap(int id, string naam, int procesId, int criteriumId)
        {
            Id = id;
            Naam = naam;
            ProcesId = procesId;
            CriteriumId = criteriumId;
        }
    }
}
