namespace StudentSysteem.Core.Models
{
    public class Vaardigheid
    {
        public int VaardigheidId { get; set; }
        public string VaardigheidNaam { get; set; }
        public string VaardigheidBeschrijving { get; set; }
        public string HboiActiviteit { get; set; }
        public string LeertakenUrl { get; set; }
        public int PrestatiedoelId { get; set; }
        public int ProcesstapId { get; set; }

        public Vaardigheid(int vaardigheidId, string vaardigheidNaam, string vaardigheidBeschrijving, string hboiActiviteit, string leertakenUrl, int prestatiedoelId, int processtapId)
        {
            VaardigheidId = vaardigheidId;
            VaardigheidNaam = vaardigheidNaam;
            VaardigheidBeschrijving = vaardigheidBeschrijving;
            HboiActiviteit = hboiActiviteit;
            LeertakenUrl = leertakenUrl;
            PrestatiedoelId = prestatiedoelId;
            ProcesstapId = processtapId;
        }
    }
}
