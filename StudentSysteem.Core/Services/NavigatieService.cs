using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.Core.Services
{
    public class NavigatieService : INavigatieService
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

