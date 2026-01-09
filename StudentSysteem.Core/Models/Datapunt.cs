namespace StudentSysteem.Core.Models
{
    public class Datapunt
    {
        public int Datapunt_id { get; set; }
        public string DatapuntNaam { get; set; }
        public string Leeruitkomst_id { get; set; }

        public Datapunt(int datapunt_id, string datapuntNaam, string leeruitkomst_id)
        {
            Datapunt_id = datapunt_id;
            DatapuntNaam = datapuntNaam;
            Leeruitkomst_id = leeruitkomst_id;
        }
    }
}