using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces
{
    public interface INavigationService
    {
        Task NavigateBackAsync();
        Task NavigateToAsync(Page page);
    }
}
