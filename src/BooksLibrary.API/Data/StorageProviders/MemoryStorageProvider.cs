using System.Collections.Generic;
using System.Linq;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.StorageProviders
{
    public class MemoryStorageProvider : IStorageProvider{
        IList<Book> books;
        IList<Author> authors;

        public MemoryStorageProvider()
        {
            books = new List<Book>();
            authors = new List<Author>();

            LoadFakeData();
        }

        void LoadFakeData(){
            var authorA = new Author{Name="Author A"};
            var authorB = new Author{Name="Author B"};

            books.Add(new Book{
                Title = "Book A",
                Authors = new List<Author>{authorA}
            });
            books.Add(new Book{
                Title = "Book B",
                Authors = new List<Author>{authorB}
            });
        }

        public Book InsertBook(Book book){
            books.Add(book);

            return book;
        }

        public void UpdateBook(string id, Book book){
            var current = books.Single(x=>x.Id.Equals(id));

            current.MapFrom(book);
        }

        public void DeleteBook(string id){
            var current = books.SingleOrDefault(x=>x.Id.Equals(id));

            if(current != null){
                books.Remove(current);
            }
        }

        public IList<Book> GetBooks(){
            return books;
        }

        public Book GetBook(string id){
            return books.SingleOrDefault(x=>x.Id.Equals(id));
        }

        public Author InsertAuthor(Author author){
            authors.Add(author);

            return author;
        }

        public void UpdateAuthor(string id, Author author){
            var current = authors.Single(x=>x.Id.Equals(id));

            current.MapFrom(author);
        }

        public void DeleteAuthor(string id){
            var current = authors.SingleOrDefault(x=>x.Id.Equals(id));

            if(current != null){
                authors.Remove(current);
            }
        }

        public IList<Author> GetAuthors(){
            return authors;
        }

        public Author GetAuthor(string id){
            return authors.SingleOrDefault(x=>x.Id.Equals(id));
        }
    }
}