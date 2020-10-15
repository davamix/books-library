using System.Collections.Generic;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Data.StorageProviders
{
    public interface IStorageProvider
    {
        void InsertBook(Book book);
        void UpdateBook(string id, Book book);
        void DeleteBook(string id);
        IList<Book> GetBooks();
        Book GetBook(string id);

        void InsertAuthor(Author author);
        void UpdateAuthor(string id, Author author);
        void DeleteAuthor(string id);
        IList<Author> GetAuthors();
        Author GetAuthor(string id);
    }
}