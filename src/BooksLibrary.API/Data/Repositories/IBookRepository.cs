using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Data.Repositories
{
    public interface IBookRepository : IDataRepository<Book>
    {
        void DeleteBookCategory(string bookId, string categoryId);
        void DeleteBookAuthor(string bookId, string authorId);
    }
}