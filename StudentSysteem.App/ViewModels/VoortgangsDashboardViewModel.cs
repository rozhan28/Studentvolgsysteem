using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using System.Collections.ObjectModel;

namespace StudentSysteem.App.ViewModels
{
    public class VoortgangsDashboardViewModel
    {
        private readonly IVaardigheidService _vaardigheidService;

        public ObservableCollection<Vaardigheid> Vaardigheden { get; }

        public VoortgangsDashboardViewModel(IVaardigheidService vaardigheidService)
        {
            _vaardigheidService = vaardigheidService;
            LaadVaardigheid();
        }

        private void LaadVaardigheid()
        {
            IEnumerable<Vaardigheid> vaardigheden = _vaardigheidService.HaalAlleVaardighedenOp();

            foreach (Vaardigheid vaardigheid in vaardigheden.Take(5))
            {
                HboiActiviteiten.Add(vaardigheid.HboiActiviteit);
            }
        }
    }
}
