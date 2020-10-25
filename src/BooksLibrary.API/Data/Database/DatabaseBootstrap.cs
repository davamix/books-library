
using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.Database
{
    public interface IDatabaseBootstrap
    {
        void Setup(bool addTestData = false);
    }

    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly IDatabaseConfiguration databaseConfiguration;

        public DatabaseBootstrap(IDatabaseConfiguration databaseConfiguration)
        {
            this.databaseConfiguration = databaseConfiguration;
            // this.configuration = configuration;
            // TODO: Initialize DB

            // Create tables 
            // -> Books
            // -> Authors
        }

        public void Setup(bool addTestData = false)
        {
            CreateTables();

            if (addTestData)
            {
                AddTestData();
            }
        }

        private void CreateTables()
        {
            CreateBookTable();
            CreateAuthorTable();
            CreateBookAuthorTable();
        }

        private void CreateBookTable()
        {
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS books(id TEXT NOT NULL UNIQUE, title TEXT NOT NULL)");
            ExecuteCommand(cmd);
        }

        private void CreateAuthorTable()
        {
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS authors(id TEXT NOT NULL UNIQUE, name TEXT NOT NULL)");
            ExecuteCommand(cmd);
        }

        private void CreateBookAuthorTable()
        {
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS book_author(book_id TEXT NOT NULL, author_id TEXT NOT NULL)");
            ExecuteCommand(cmd);
        }

        private void ExecuteCommand(SqliteCommand command)
        {
            using (var connection = new SqliteConnection(databaseConfiguration.GetConnectionString()))
            {
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        // private T ExecuteScalar<T>(SqliteCommand command)
        // {
        //     using (var connection = new SqliteConnection(this.connectionString))
        //     {
        //         try
        //         {
        //             command.Connection = connection;
        //             connection.Open();
        //             T result = (T)command.ExecuteScalar();
        //             return result;
        //         }
        //         catch (SqliteException ex)
        //         {
        //             System.Console.WriteLine(ex.Message);
        //         }
        //     }

        //     return default(T);
        // }

        private void AddTestData()
        {
            var bookA_id = "dd9e7cf3-0519-47e5-ba04-7d061c0cabec";
            var bookB_id = "3faab695-e267-432c-96ed-cf5b28c8df8f";
            var authorA_id = "fc3cf08a-9ac5-41fa-8282-e7974a65c868";
            var authorB_id = "e95f4ae7-7a7e-4523-858a-4bf8e4512b67";

            ExecuteCommand(new SqliteCommand($"INSERT INTO books(id, title) VALUES('{bookA_id}', 'Book A');"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO books(id, title) VALUES('{bookB_id}', 'Book B');"));

            ExecuteCommand(new SqliteCommand($"INSERT INTO authors(id, name) VALUES('{authorA_id}', 'Author A');"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO authors(id, name) VALUES('{authorB_id}', 'Author B');"));

            ExecuteCommand(new SqliteCommand($"INSERT INTO book_author(book_id, author_id) VALUES('{bookA_id}', '{authorA_id}')"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO book_author(book_id, author_id) VALUES('{bookB_id}', '{authorB_id}')"));
        }

    }
}