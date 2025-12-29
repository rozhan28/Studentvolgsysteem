namespace StudentSysteem.Core.Models;

public class Docent : Gebruiker
{
    public string ClusterId { get; set; }
    
    public Docent(int id, string naam, string email, string nummer) : base(naam, Role.Docent)
    {
        Id = id;
        Email = email;
        Nummer = nummer;
    }
}