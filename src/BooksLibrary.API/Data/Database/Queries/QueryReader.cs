using System;
using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.Database.Queries
{
    public class QueryReader : IQueryReader{
        private readonly IDatabaseConfiguration databaseConfiguration;

        public QueryReader(IDatabaseConfiguration databaseConfiguration)
        {
            this.databaseConfiguration = databaseConfiguration;
        }

        public T Execute<T>(string query, Func<SqliteDataReader, T> mapper)
        {
            var cmd = new SqliteCommand(query);

            using (var connection = new SqliteConnection(databaseConfiguration.GetConnectionString())){
                try{
                    cmd.Connection = connection;
                    connection.Open();

                    var reader = cmd.ExecuteReader();
                    
                    return mapper(reader);

                }catch(SqliteException){
                    throw;
                }
            }
        }
    }
}