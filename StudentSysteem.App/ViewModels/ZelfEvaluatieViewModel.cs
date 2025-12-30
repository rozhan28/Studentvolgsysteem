using StudentSysteem.Core.Models;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.App.ViewModels
{
    public partial class ZelfEvaluatieViewModel : BasisViewModel
    {
        private readonly IZelfEvaluatieService _zelfEvaluatieService;

        public ZelfEvaluatieViewModel(IZelfEvaluatieService zelfEvaluatieService)
        {
            _zelfEvaluatieService = zelfEvaluatieService;
        }

        public int SlaZelfEvaluatieOp(int studentId)
        {
            ZelfEvaluatie zelfEvaluatie = new ZelfEvaluatie
            {
                StudentId = studentId,
                Datum = DateTime.Now,
                PrestatieNiveau = "Geselecteerd" 
            };
            return _zelfEvaluatieService.VoegToe(zelfEvaluatie);
        }
    }
}
