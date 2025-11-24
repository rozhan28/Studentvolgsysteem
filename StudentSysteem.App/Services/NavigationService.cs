using System;
using System.Collections.Generic;
using System.Text;

namespace StudentVolgSysteem.App.Services
{
    public class NavigationService : INavigationService
    {
        public Task NavigateBackAsync()
        {
            return Application.Current.MainPage.Navigation.PopAsync();
        }

        public Task NavigateToAsync(Page page)
        {
            return Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}

