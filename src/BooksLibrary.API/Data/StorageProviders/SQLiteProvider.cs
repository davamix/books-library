using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.Database.Queries;

namespace BooksLibrary.API.Data.StorageProviders
{
    public partial class SQLiteProvider : IStorageProvider
    {
        private readonly IDatabaseConfiguration databaseConfiguration;
        private readonly IQueryReader queryReader;
        private readonly IQueryCommand queryCommand;

        public SQLiteProvider(IDatabaseConfiguration databaseConfiguration, IQueryReader queryReader, IQueryCommand queryCommand)
        {
            this.databaseConfiguration = databaseConfiguration;
            this.queryReader = queryReader;
            this.queryCommand = queryCommand;
        }
    }
}