using StudentSysteem.Core.Models;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.App.ViewModels
{
    public class ZelfEvaluatieViewModel
    {
        private readonly IZelfEvaluatieService _zelfEvaluatieService;

        public ZelfEvaluatieViewModel(IZelfEvaluatieService zelfEvaluatieService)
        {
            _zelfEvaluatieService = zelfEvaluatieService;
        }

        public int SlaZelfEvaluatieOp(int studentId)
        {
            var zelfEvaluatie = new ZelfEvaluatie
            {
                StudentId = studentId,
                Datum = DateTime.Now,
                PrestatieNiveau = "Geselecteerd" 
            };

            return _zelfEvaluatieService.Add(zelfEvaluatie);
        }
    }
}
