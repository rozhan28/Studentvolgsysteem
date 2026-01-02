namespace StudentSysteem.Core.Models;

public class Feedback
{
    public int Feedback_id { get; set; }
    public string Niveauaanduiding { get; set; }
    public string Toelichting { get; set; }
    public string Datum { get; set; }
    public string Tijd { get; set; }
    public int Student_id { get; set; }
    public int Docent_id { get; set; }
    public int Vaardigheid_id { get; set; }
    public int Datapunt_id { get; set; }

    public Feedback(
        int feedback_id,
        string niveauaanduiding,
        string toelichting,
        string datum,
        string tijd,
        int student_id,
        int docent_id,
        int vaardigheid_id,
        int datapunt_id)
    {
        Feedback_id = feedback_id;
        Niveauaanduiding = niveauaanduiding;
        Toelichting = toelichting;
        Datum = datum;
        Tijd = tijd;
        Student_id = student_id;
        Docent_id = docent_id;
        Vaardigheid_id = vaardigheid_id;
        Datapunt_id = datapunt_id;
    }
}