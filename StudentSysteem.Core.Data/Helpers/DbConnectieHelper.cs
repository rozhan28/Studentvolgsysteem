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
        
        public string ConnectionStringValue(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }
}