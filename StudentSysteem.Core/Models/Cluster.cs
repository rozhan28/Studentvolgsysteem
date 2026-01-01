namespace StudentSysteem.Core.Models;

public class Cluster
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public string Code { get; set; }

    public Cluster(int id, string naam, string code)
    {
        Id = id;
        Naam = naam;
        Code = code;
    }
}