namespace StudentVolgSysteem.Core.Services
{
    public static class GebruikerSessie
    {
        public static string HuidigeRol { get; private set; } = "";

        public static void LoginAls(string rol)
        {
            HuidigeRol = rol;
        }

        public static void Loguit()
        {
            HuidigeRol = "";
        }
    }
}

