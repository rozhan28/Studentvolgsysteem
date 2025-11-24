using System;
using System.Collections.Generic;
using System.Text;

namespace StudentVolgSysteem.App.Services
{
    public interface IAlertService
    {
        Task ShowAlertAsync(string title, string message, string okText = "OK");
    }
}

