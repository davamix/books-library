using System.Collections.Generic;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.StorageProviders;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.StorageProviders
{
    public class SQLiteProvider : IStorageProvider
    {
        private readonly IDatabaseConfiguration databaseConfiguration;

        public SQLiteProvider(IDatabaseConfiguration databaseConfiguration)
        {
            this.databaseConfiguration = databaseConfiguration;
        }

        public void DeleteAuthor(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteBook(string id)
        {
            throw new System.NotImplementedException();
        }

        public Author GetAuthor(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Author> GetAuthors()
        {
            throw new System.NotImplementedException();
        }

        public Book GetBook(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Book> GetBooks()
        {
            var books = new List<Book>();
            var query = @"SELECT b.id AS book_id, b.title, a.id, a.name
                        FROM books AS b INNER JOIN book_author 
                        ON b.id = book_author.book_id 
                        INNER JOIN authors AS a 
                        ON book_author.author_id = a.id;";
            var cmd = new SqliteCommand(query);

            using (var connection = new SqliteConnection(databaseConfiguration.GetConnectionString()))
            {
                try
                {
                    cmd.Connection = connection;
                    connection.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetString(0),
                            Title = reader.GetString(1),
                            Authors = new List<Author>(){
                                new Author{
                                    Id = reader.GetString(2),
                                    Name = reader.GetString(3)}
                            }
                        });
                    }
                }
                catch (SqliteException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }

                return books;
            }
        }

        public Author InsertAuthor(Author author)
        {
            throw new System.NotImplementedException();
        }

        public Book InsertBook(Book book)
        {
            throw new System.NotImplementedException();
        }

        public IList<Author> SearchAuthor(string query)
        {
            throw new System.NotImplementedException();
        }

        public IList<Book> SearchBook(string query)
        {
            throw new System.NotImplementedException();
        }

        public Author UpdateAuthor(string id, Author author)
        {
            throw new System.NotImplementedException();
        }

        public Book UpdateBook(string id, Book book)
        {
            throw new System.NotImplementedException();
        }
    }
}