using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.Database.Queries
{
    public class QueryCommand : IQueryCommand
    {
        private readonly IDatabaseConfiguration databaseConfiguration;

        public QueryCommand(IDatabaseConfiguration databaseConfiguration)
        {
            this.databaseConfiguration = databaseConfiguration;
        }

        public void Execute(string query)
        {
            var cmd = new SqliteCommand(query);

            using (var connection = new SqliteConnection(databaseConfiguration.GetConnectionString())){
                try{
                    cmd.Connection = connection;
                    connection.Open();
                    
                    cmd.ExecuteNonQuery();

                }catch(SqliteException){
                    throw;
                }
            }
            
        }
    }
}