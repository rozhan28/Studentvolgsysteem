using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Models
{
    public class Vaardigheid
    {
        public int Vaardigheid_id { get; set; }
        public string VaardigheidNaam { get; set; }
        public string VaardigheidBeschrijving { get; set; }
        public string LeertakenUrl { get; set; }
        public int Prestatiedoel_id { get; set; }
        public int Leeruitkomst_id { get; set; }
        public int Processtap_id { get; set; }
        public string HboiActiviteit { get; set; }

        public Vaardigheid(int vaardigheid_id, string vaardigheidNaam, string vaardigheidBeschrijving, string leertakenUrl, int prestatiedoelId, int leeruitkomstId, string hboiActiviteit)
        {
            Vaardigheid_id = vaardigheid_id;
            VaardigheidNaam = vaardigheidNaam;
            VaardigheidBeschrijving = vaardigheidBeschrijving;
            LeertakenUrl = leertakenUrl;
            Prestatiedoel_id = prestatiedoelId;
            Leeruitkomst_id = leeruitkomstId;
            HboiActiviteit = hboiActiviteit;
        }

    }
}
