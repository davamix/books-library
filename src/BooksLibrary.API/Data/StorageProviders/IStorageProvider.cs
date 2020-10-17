using System.Collections.Generic;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Data.StorageProviders
{
    public interface IStorageProvider
    {
        Book InsertBook(Book book);
        void UpdateBook(string id, Book book);
        void DeleteBook(string id);
        IList<Book> GetBooks();
        Book GetBook(string id);
        IList<Book> SearchBook(string query);

        Author InsertAuthor(Author author);
        void UpdateAuthor(string id, Author author);
        void DeleteAuthor(string id);
        IList<Author> GetAuthors();
        Author GetAuthor(string id);
        IList<Author> SearchAuthor(string query);
    }
}