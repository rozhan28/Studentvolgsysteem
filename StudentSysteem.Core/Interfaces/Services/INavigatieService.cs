namespace StudentSysteem.Core.Interfaces.Services
{
    public interface INavigatieService
    {
        Task NavigateBackAsync();
        Task NavigateToAsync(Page page);
    }
}