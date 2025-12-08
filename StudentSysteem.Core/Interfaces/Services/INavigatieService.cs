using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface INavigatieService
    {
        Task NavigateBackAsync();
        Task NavigateToAsync(Page page);
    }
}
