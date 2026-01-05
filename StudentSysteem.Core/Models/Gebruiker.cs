namespace StudentSysteem.Core.Models;

public class Gebruiker
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public string Email { get; set; }
    public int Nummer { get; set; }
    public Role Rol { get; set; }

    public Gebruiker(string naam, Role rol)
    {
        Naam = naam;
        Rol = rol;
    }
}
