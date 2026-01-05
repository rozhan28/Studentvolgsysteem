using Microsoft.Extensions.Configuration;

namespace StudentSysteem.Core.Data.Helpers
{
    public class DbConnectieHelper
    {
        private readonly IConfiguration _configuration;
        
        public DbConnectieHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string ConnectieStringWaarde(string naam)
        {
            string connectieString = _configuration.GetConnectionString(naam);
            
            if (string.IsNullOrWhiteSpace(connectieString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{naam}' is niet geconfigureerd in appsettings.json.");
            }
            return connectieString;
        }
    }
}