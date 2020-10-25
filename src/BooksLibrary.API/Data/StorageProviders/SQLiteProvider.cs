using System;
using System.Collections.Generic;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.Database.Queries;
using BooksLibrary.API.Data.StorageProviders;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.StorageProviders
{
    public class SQLiteProvider : IStorageProvider
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

        public void DeleteAuthor(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteBook(string id)
        {
            // QUERY
            var query = $@"BEGIN;
                        DELETE FROM book_author WHERE book_id = '{id}';
                        DELETE FROM books WHERE id = '{id}';
                        COMMIT;";

            // EXECUTE
            try{
                queryCommand.Execute(query);
            }catch(SqliteException){
                throw;
            }
        }

        public Author GetAuthor(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Author> GetAuthors()
        {
            // QUERY
            var query = @"SELECT id, name FROM authors;";

            // MAPPER
            Func<SqliteDataReader, List<Author>> mapper = (SqliteDataReader reader) =>
            {
                var authors = new List<Author>();

                while (reader.Read())
                {
                    authors.Add(new Author
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1)
                    });
                }

                return authors;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }

        public Book GetBook(string id)
        {
            // QUERY
            var query = $@"SELECT b.id AS book_id, b.title, a.id, a.name
                        FROM books AS b INNER JOIN book_author 
                        ON b.id = book_author.book_id 
                        INNER JOIN authors AS a 
                        ON book_author.author_id = a.id
                        WHERE b.id = '{id}'";

            // MAPPER
            Func<SqliteDataReader, Book> mapper = (SqliteDataReader reader) =>
            {
                while (reader.Read())
                {
                    return new Book
                    {
                        Id = reader.GetString(0),
                        Title = reader.GetString(1),
                        Authors = new List<Author>{
                                new Author{
                                    Id = reader.GetString(2),
                                    Name = reader.GetString(3)
                                }
                        }
                    };
                }

                throw new ArgumentException();
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }

        public IList<Book> GetBooks()
        {
            // QUERY
            var query = @"SELECT b.id AS book_id, b.title, a.id, a.name
                        FROM books AS b INNER JOIN book_author 
                        ON b.id = book_author.book_id 
                        INNER JOIN authors AS a 
                        ON book_author.author_id = a.id;";

            // MAPPER
            Func<SqliteDataReader, IList<Book>> mapper = (SqliteDataReader reader) =>
            {
                var books = new List<Book>();

                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetString(0),
                        Title = reader.GetString(1),
                        Authors = new List<Author>(){
                                new Author{
                                    Id = reader.GetString(2),
                                    Name = reader.GetString(3)
                                }
                        }
                    });
                }

                return books;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }

        public Author InsertAuthor(Author author)
        {
            throw new System.NotImplementedException();
        }

        public Book InsertBook(Book book)
        {
            // QUERY

            var bookAuthorQueries = new List<string>();

            foreach (var author in book.Authors)
            {
                bookAuthorQueries.Add($"INSERT INTO book_author(book_id, author_id) VALUES('{book.Id}', '{author.Id}');");
            }

            var query = $@"BEGIN; 
                        INSERT INTO books(id, title) VALUES('{book.Id}', '{book.Title}');
                        {string.Join(string.Empty, bookAuthorQueries)}
                        COMMIT;";

            // EXECUTE
            try{
                queryCommand.Execute(query);
            }catch(SqliteException){
                throw;
            }
            
            return book;
        }

        public IList<Author> SearchAuthor(string query)
        {
            throw new System.NotImplementedException();
        }

        public IList<Book> SearchBook(string query)
        {
            // QUERY
            var q = $@"SELECT b.id AS book_id, b.title, a.id, a.name
                        FROM books AS b INNER JOIN book_author 
                        ON b.id = book_author.book_id 
                        INNER JOIN authors AS a 
                        ON book_author.author_id = a.id 
                        WHERE b.title LIKE '{query}';";

            // MAPPER
            Func<SqliteDataReader, IList<Book>> mapper = (SqliteDataReader reader) =>{
                var books = new List<Book>();

                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetString(0),
                        Title = reader.GetString(1),
                        Authors = new List<Author>(){
                                new Author{
                                    Id = reader.GetString(2),
                                    Name = reader.GetString(3)
                                }
                        }
                    });
                }

                return books;
            };

            return queryReader.Execute(q, mapper);
        }

        public Author UpdateAuthor(string id, Author author)
        {
            throw new System.NotImplementedException();
        }

        public Book UpdateBook(string id, Book book)
        {
            // QUERY
            var bookAuthorQueries = new List<string>();

            foreach (var author in book.Authors)
            {
                bookAuthorQueries.Add($"INSERT INTO book_author(book_id, author_id) VALUES('{id}', '{author.Id}');");
            }

            var query = $@"BEGIN;
                        DELETE FROM book_author WHERE book_id = '{id}';
                        UPDATE books SET title = '{book.Title}' WHERE id = '{id}';
                        {string.Join(string.Empty, bookAuthorQueries)}
                        COMMIT;";

            // EXECUTE
            try{
                queryCommand.Execute(query);
            }catch(SqliteException){
                throw;
            }

            return book;
        }
    }
}