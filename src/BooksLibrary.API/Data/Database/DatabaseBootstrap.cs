
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
            CreateBooksTable();
            CreateAuthorsTable();
            CreateBookAuthorTable();
            CreateCategoriesTable();
            CreateBookCategoryTable();
        }

        private void CreateBooksTable()
        {
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS books(id TEXT NOT NULL UNIQUE, title TEXT NOT NULL, image TEXT)");
            ExecuteCommand(cmd);
        }

        private void CreateAuthorsTable()
        {
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS authors(id TEXT NOT NULL UNIQUE, name TEXT NOT NULL)");
            ExecuteCommand(cmd);
        }

        private void CreateBookAuthorTable()
        {
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS book_author(book_id TEXT NOT NULL, author_id TEXT NOT NULL)");
            ExecuteCommand(cmd);
        }

        private void CreateCategoriesTable(){
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS categories(id TEXT NOT NULL, name TEXT NOT NULL)");
            ExecuteCommand(cmd);
        }

        private void CreateBookCategoryTable(){
            var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS book_category(book_id TEXT NOT NULL, category_id TEXT NOT NULL)");
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

        private void AddTestData()
        {
            var bookA_id = "dd9e7cf3-0519-47e5-ba04-7d061c0cabec";
            var bookB_id = "3faab695-e267-432c-96ed-cf5b28c8df8f";
            var authorA_id = "fc3cf08a-9ac5-41fa-8282-e7974a65c868";
            var authorB_id = "e95f4ae7-7a7e-4523-858a-4bf8e4512b67";
            var categoryA_id = "0f3e2102-23a9-4f19-9181-2a916fd40c35";
            var categoryB_id = "6cc6461c-61d8-4b18-9fd7-40af65b1f21b";

            ExecuteCommand(new SqliteCommand($"INSERT INTO books(id, title) VALUES('{bookA_id}', 'Book A');"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO books(id, title) VALUES('{bookB_id}', 'Book B');"));

            ExecuteCommand(new SqliteCommand($"INSERT INTO authors(id, name) VALUES('{authorA_id}', 'Author A');"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO authors(id, name) VALUES('{authorB_id}', 'Author B');"));

            ExecuteCommand(new SqliteCommand($"INSERT INTO categories(id, name) VALUES('{categoryA_id}', 'Category A');"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO categories(id, name) VALUES('{categoryB_id}', 'Category B');"));

            ExecuteCommand(new SqliteCommand($"INSERT INTO book_author(book_id, author_id) VALUES('{bookA_id}', '{authorA_id}');"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO book_author(book_id, author_id) VALUES('{bookB_id}', '{authorB_id}');"));

            ExecuteCommand(new SqliteCommand($"INSERT INTO book_category(book_id, category_id) VALUES('{bookA_id}', '{categoryA_id}');"));
            ExecuteCommand(new SqliteCommand($"INSERT INTO book_category(book_id, category_id) VALUES('{bookB_id}', '{categoryB_id}');"));
        }

    }
}
