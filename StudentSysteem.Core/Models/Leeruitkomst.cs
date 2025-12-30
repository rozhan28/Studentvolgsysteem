namespace StudentSysteem.Core.Models;

public class Leeruitkomst
{
    public int Leeruitkomst_id { get; set; }
    public string Naam { get; set; }
    public string HboiActiviteit { get; set; }
    public int Datapunt_id { get; set; }

    public Leeruitkomst(int leeruitkomst_id, string naam, string hboiActiviteit)
    {
        Leeruitkomst_id = leeruitkomst_id;
        Naam = naam;
        HboiActiviteit = hboiActiviteit;
    }
}