namespace StudentSysteem.Core.Models
{
    // 'Pakketje' voor de FormulierViewModel
    public class BeoordelingStructuur
    {
        public Proces Proces { get; set; }
        public Processtap Processtap { get; set; }
        public Vaardigheid Vaardigheid { get; set; }
        public Prestatiedoel Prestatiedoel { get; set; }

        public BeoordelingStructuur(Proces proces, Processtap processtap, Vaardigheid vaardigheid, Prestatiedoel prestatiedoel)
        {
            Proces = proces;
            Processtap = processtap;
            Vaardigheid = vaardigheid;
            Prestatiedoel = prestatiedoel;
        }
    }
}