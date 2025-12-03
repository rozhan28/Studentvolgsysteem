using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Data.Repositories
{
    public class VaardigheidtrappetjeComposite
    {
        VaardigheidtrappetjeComposite()
        {
            ProcesRepository ProcesRepository = new ProcesRepository();
            ProcesstapRepository ProcesstapRepository = new ProcesstapRepository();
            VaardigheidRepository VaardigheidRepository = new VaardigheidRepository();
            PrestatiedoelRepository prestatiedoelRepository = new PrestatiedoelRepository();

        }
    }
}
