using StudentSysteem.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentVolgSysteem.App.Services
{
    public class MeldingService : IMeldingService
    {
        public Task ToonMeldingAsync(string title, string message, string okText = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, okText);
        }
    }
}

