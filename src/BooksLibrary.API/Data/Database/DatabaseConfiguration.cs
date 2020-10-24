using Microsoft.Extensions.Configuration;
using System.IO;

namespace BooksLibrary.API.Data.Database
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        private readonly IConfiguration configuration;

        public DatabaseConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetConnectionString()
        {
            return string.Format(configuration["ConnectionStrings:SQLiteConnection"], Directory.GetCurrentDirectory());
        }
    }
}