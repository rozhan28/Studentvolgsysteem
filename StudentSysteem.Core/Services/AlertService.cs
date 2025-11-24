using StudentSysteem.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentVolgSysteem.App.Services
{
    public class AlertService : IAlertService
    {
        public Task ShowAlertAsync(string title, string message, string okText = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, okText);
        }
    }
}

