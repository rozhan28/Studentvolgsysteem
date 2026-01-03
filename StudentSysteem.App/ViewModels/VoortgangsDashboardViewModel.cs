using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using System.Collections.ObjectModel;

namespace StudentSysteem.App.ViewModels
{
    public class VoortgangsDashboardViewModel
    {
        private readonly ILeeruitkomstService _leeruitkomstService;

        public ObservableCollection<Leeruitkomst> Leeruitkomsten { get; } = new ObservableCollection<Leeruitkomst>();

        public VoortgangsDashboardViewModel(ILeeruitkomstService leeruitkomstService)
        {
            _leeruitkomstService = leeruitkomstService;
            LaadLeeruitkomsten();
        }

        private void LaadLeeruitkomsten()
        {
            IEnumerable<Leeruitkomst> leeruitkomstService = _leeruitkomstService.HaalAlleLeeruitkomstenOp();

            // Beperk het dashboard om maximaal 5 HBO-i-activiteit te laten zien in de tabel
            foreach (Leeruitkomst leeruitkomst in leeruitkomstService.Take(5))
            {
                Leeruitkomsten.Add(leeruitkomst);
            }
        }
    }
}