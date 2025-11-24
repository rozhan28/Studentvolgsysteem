using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IAlertService
    {
        Task ShowAlertAsync(string title, string message, string okText = "OK");
    }
}

