using System;
using System.Collections.Generic;
using System.Text;

namespace StudentVolgSysteem.App.Services
{
    public interface INavigationService
    {
        Task NavigateBackAsync();
        Task NavigateToAsync(Page page);
    }
}
