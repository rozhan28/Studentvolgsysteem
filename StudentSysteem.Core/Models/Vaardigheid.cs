namespace StudentSysteem.Core.Models
{
    public class Vaardigheid
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public string LeertakenUrl { get; set; }
        public int PrestatiedoelId { get; set; }
        public int ProcesstapId { get; set; }
        public int LeeruitkomstId { get; set; }
        public string HboiActiviteit { get; set; }

        public Vaardigheid() { }

        public Vaardigheid(int id, string naam, string beschrijving, string leertakenUrl,
                           int prestatiedoelId, int leeruitkomstId, string hboiActiviteit)
        {
            Id = id;
            Naam = naam;
            Beschrijving = beschrijving;
            LeertakenUrl = leertakenUrl;
            PrestatiedoelId = prestatiedoelId;
            LeeruitkomstId = leeruitkomstId;
            HboiActiviteit = hboiActiviteit;
        }

        public string VaardigheidNaam => Naam;
        public string VaardigheidBeschrijving => Beschrijving;
    }
}
