using System;
using System.Collections.Generic;
using System.Linq;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;
using BooksLibrary.API.Data.Database.Extensions;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.Database.Queries;

namespace BooksLibrary.API.Data.StorageProviders.SQLiteProvider
{
    public class BookStorageProvider : SQLiteProviderBase<Book>, IBookStorageProvider
    {
        public BookStorageProvider(IDatabaseConfiguration databaseConfiguration, IQueryReader queryReader, IQueryCommand queryCommand)
            : base(databaseConfiguration, queryReader, queryCommand)
        { }

        public override Book Get(string id)
        {
            // QUERY
            var query = $@"SELECT b.id AS book_id, b.title, b.image, a.id, a.name, c.id, c.name
                        FROM books AS b INNER JOIN book_author 
                        ON b.id = book_author.book_id 
                        INNER JOIN authors AS a 
                        ON book_author.author_id = a.id
                        LEFT JOIN book_category 
                        ON b.id = book_category.book_id
                        LEFT JOIN categories as c
                        ON c.id = book_category.category_id
                        WHERE b.id = '{id}'";

            // MAPPER
            Func<SqliteDataReader, Book> mapper = (SqliteDataReader reader) =>
            {
                var book = new Book();

                try
                {
                    while (reader.Read())
                    {

                        book.Id = reader.GetValue<string>(0);
                        book.Title = reader.GetValue<string>(1);
                        book.Image = reader.GetValue<string>(2, true);

                        var authorId = reader.GetValue<string>(3);
                        if (!book.Authors.Any(x => x.Id.Equals(authorId)))
                        {
                            book.Authors.Add(new Author
                            {
                                Id = authorId,
                                Name = reader.GetValue<string>(4)
                            });
                        }

                        var categoryId = reader.GetValue<string>(5, true);
                        if (categoryId != null && !book.Categories.Any(x => x.Id.Equals(categoryId)))
                        {
                            book.Categories.Add(new Category
                            {
                                Id = categoryId,
                                Name = reader.GetValue<string>(6, true)
                            });
                        }
                    }
                }
                catch (SqliteException)
                {
                    throw;
                }
                catch (ArgumentException)
                {
                    throw;
                }

                return book;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }

        public override IList<Book> Get()
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
                var books = new Dictionary<string, Book>();

                while (reader.Read())
                {
                    var book = new Book();
                    var bookId = reader.GetValue<string>(0);

                    if (books.ContainsKey(bookId))
                    {
                        book = books[bookId];
                    }

                    book.Id = bookId;
                    book.Title = reader.GetValue<string>(1);
                    book.Image = reader.GetValue<string>(2, true);
                    book.Authors.Add(new Author
                    {
                        Id = reader.GetValue<string>(3),
                        Name = reader.GetValue<string>(4)
                    });

                    books[bookId] = book;
                }

                return books.Select(x => x.Value).ToList();
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }

        protected override void InsertImp(Book book)
        {
            // QUERY
            var bookAuthorQueries = new List<string>();
            var bookCategoryQueries = new List<string>();

            foreach (var author in book.Authors)
            {
                bookAuthorQueries.Add($"INSERT INTO book_author(book_id, author_id) VALUES('{book.Id}', '{author.Id}');");
            }

            foreach (var category in book.Categories)
            {
                bookCategoryQueries.Add($"INSERT INTO book_category(book_id, category_id) VALUES('{book.Id}', '{category.Id}');");
            }

            var query = $@"BEGIN; 
                        INSERT INTO books(id, title, image) VALUES('{book.Id}', '{book.Title}', '{book.Image}');
                        {string.Join(string.Empty, bookAuthorQueries)}
                        {string.Join(string.Empty, bookCategoryQueries)}
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

        public override Book Update(string id, Book book)
        {
            // QUERY
            var bookAuthorQueries = new List<string>();
            var bookCategoryQueries = new List<string>();

            foreach (var author in book.Authors)
            {
                bookAuthorQueries.Add($"INSERT INTO book_author(book_id, author_id) VALUES('{id}', '{author.Id}');");
            }

            foreach (var category in book.Categories)
            {
                bookCategoryQueries.Add($"INSERT INTO book_category(book_id, category_id) VALUES('{id}', '{category.Id}');");
            }

            var query = $@"BEGIN;
                        DELETE FROM book_author WHERE book_id = '{id}';
                        DELETE FROM book_category WHERE book_id = '{id}';
                        UPDATE books SET title = '{book.Title}', image = '{book.Image}' WHERE id = '{id}';
                        {string.Join(string.Empty, bookAuthorQueries)};
                        {string.Join(string.Empty, bookCategoryQueries)};
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

        public override void Delete(string id)
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

        public override IList<Book> Search(string query)
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

        public void DeleteBookCategory(string bookId, string categoryId)
        {
            // QUERY
            var query = $"DELETE FROM book_category WHERE book_id = '{bookId}' AND category_id = '{categoryId}'";

            // EXECUTE
            queryCommand.Execute(query);
        }

        public void DeleteBookAuthor(string bookId, string authorId)
        {
            // QUERY
            var query = $"DELETE FROM book_author WHERE book_id = '{bookId}' AND author_id = '{authorId}'";

            // EXECUTE
            queryCommand.Execute(query);
        }
    }
}