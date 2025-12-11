namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IMeldingService
    {
        Task ToonMeldingAsync(string title, string message, string okText = "OK");
    }
}