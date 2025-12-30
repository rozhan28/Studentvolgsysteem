namespace StudentSysteem.Core.Models;

public class Student : Gebruiker
{
    public string Klas { get; set; }
    
    public Student(int id, string naam, string email, int nummer, string klas) : base(naam, Role.Student)
    {
        Id = id;
        Email = email;
        Nummer = nummer;
        Klas = klas;
    }
}