namespace StudentSysteem.Core.Models
{
    // 'Pakketje' voor de FeedbackformulierViewModel
    public class BeoordelingStructuur
    {
        public Proces Proces { get; set; }
        public Processtap Stap { get; set; }
        public Vaardigheid Vaardigheid { get; set; }
        public Prestatiedoel Doel { get; set; }

        public BeoordelingStructuur(Proces proces, Processtap stap, Vaardigheid vaardigheid, Prestatiedoel doel)
        {
            Proces = proces;
            Stap = stap;
            Vaardigheid = vaardigheid;
            Doel = doel;
        }
    }
}