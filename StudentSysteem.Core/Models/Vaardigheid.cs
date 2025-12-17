using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Models
{
    public class Vaardigheid
    {
        //US2 properties
        public int Vaardigheid_id { get; set; }
        public string VaardigheidNaam { get; set; }
        public string VaardigheidBeschrijving { get; set; }
        public string HboiActiviteit { get; set; }
        public string LeertakenUrl { get; set; }

        //Wat moet ik hiermee??
        public int Prestatiedoel_id { get; set; }
        public int Processtap_id { get; set; }

        public Vaardigheid(int vaardigheid_id, string vaardigheidNaam, string vaardigheidBeschrijving, string hboiActiviteit, string leertakenUrl)
        {
            Vaardigheid_id = vaardigheid_id;
            VaardigheidNaam = vaardigheidNaam;
            VaardigheidBeschrijving = vaardigheidBeschrijving;
            HboiActiviteit = hboiActiviteit;
            LeertakenUrl = leertakenUrl;
        }

    }
}
