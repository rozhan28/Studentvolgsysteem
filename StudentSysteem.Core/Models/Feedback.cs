namespace StudentSysteem.Core.Models;

public class Feedback
{
    public Niveauaanduiding Niveauaanduiding { get; set; }
    public int StudentId { get; set; } = 0;
    public int FeedbackGeverId { get; set; } = 0;
    public int DocentId { get; set; } = 0;
    public List<Toelichting> Toelichtingen { get; set; }
    public int VaardigheidId { get; set; }

    public Feedback(int vaardigheidId)
    {
        VaardigheidId = vaardigheidId;
    }
}