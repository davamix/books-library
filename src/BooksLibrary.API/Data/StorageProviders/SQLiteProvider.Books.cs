using System;
using System.Collections.Generic;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;
using BooksLibrary.API.Data.Database.Extensions;

namespace BooksLibrary.API.Data.StorageProviders
{
    public partial class SQLiteProvider : IStorageProvider
    {
        public Book GetBook(string id)
        {
            // QUERY
            var query = $@"SELECT b.id AS book_id, b.title, b.image, a.id, a.name
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
                        Id = reader.GetValue<string>(0),
                        Title = reader.GetValue<string>(1),
                        Image = reader.GetValue<string>(2, true),
                        Authors = new List<Author>{
                                new Author{
                                    Id = reader.GetValue<string>(3),
                                    Name = reader.GetValue<string>(4)
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
            var query = @"SELECT b.id AS book_id, b.title, b.image, a.id, a.name
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
                        Id = reader.GetValue<string>(0),
                        Title = reader.GetValue<string>(1),
                        Image = reader.GetValue<string>(2, true),
                        Authors = new List<Author>(){
                                new Author{
                                    Id = reader.GetValue<string>(3),
                                    Name = reader.GetValue<string>(4)
                                }
                        }
                    });
                }

                return books;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
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
                        INSERT INTO books(id, title, image) VALUES('{book.Id}', '{book.Title}', '{book.Image}');
                        {string.Join(string.Empty, bookAuthorQueries)}
                        COMMIT;";

            // EXECUTE
            try
            {
                queryCommand.Execute(query);
            }
            catch (SqliteException)
            {
                throw;
            }

            return book;
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
                        UPDATE books SET title = '{book.Title}', image = '{book.Image}' WHERE id = '{id}';
                        {string.Join(string.Empty, bookAuthorQueries)}
                        COMMIT;";

            // EXECUTE
            try
            {
                queryCommand.Execute(query);
            }
            catch (SqliteException)
            {
                throw;
            }

            book.Id = id;
            return book;
        }

        public void DeleteBook(string id)
        {
            // QUERY
            var query = $@"BEGIN;
                        DELETE FROM book_author WHERE book_id = '{id}';
                        DELETE FROM books WHERE id = '{id}';
                        COMMIT;";

            // EXECUTE
            try
            {
                queryCommand.Execute(query);
            }
            catch (SqliteException)
            {
                throw;
            }
        }

        public IList<Book> SearchBook(string query)
        {
            // QUERY
            var q = $@"SELECT b.id AS book_id, b.title, b.image, a.id, a.name
                        FROM books AS b INNER JOIN book_author 
                        ON b.id = book_author.book_id 
                        INNER JOIN authors AS a 
                        ON book_author.author_id = a.id 
                        WHERE b.title LIKE '%{query}%';";

            // MAPPER
            Func<SqliteDataReader, IList<Book>> mapper = (SqliteDataReader reader) =>
            {
                var books = new List<Book>();

                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetValue<string>(0),
                        Title = reader.GetValue<string>(1),
                        Image = reader.GetValue<string>(2, true),
                        Authors = new List<Author>(){
                                new Author{
                                    Id = reader.GetValue<string>(3),
                                    Name = reader.GetValue<string>(4)
                                }
                        }
                    });
                }

                return books;
            };

            // EXECUTE
            return queryReader.Execute(q, mapper);
        }
    }
}