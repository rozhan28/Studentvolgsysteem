using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.Core.Services
{
    public class MeldingService : IMeldingService
    {
        public Task ToonMeldingAsync(string title, string message, string okText = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, okText);
        }
    }
}