namespace StudentVolgSysteem.Core.Models
{
    public class ZelfReflectie
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        public string PrestatieNiveau { get; set; } = string.Empty;
        public string Toelichting { get; set; } = string.Empty;

        public DateTime Datum { get; set; } = DateTime.Now;
    }
}

