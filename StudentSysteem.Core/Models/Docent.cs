namespace StudentSysteem.Core.Models;

public class Docent : Gebruiker
{
    public int ClusterId { get; set; }
    
    public Docent(int id, string naam, string email, int nummer, int clusterId) : base(naam, Role.Docent)
    {
        Id = id;
        Email = email;
        Nummer = nummer;
        ClusterId = clusterId;
    }
}