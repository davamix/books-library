using System.Collections.Generic;
using System.Linq;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.StorageProviders
{
    public class MemoryProvider : IStorageProvider{
        IList<Book> books;
        IList<Author> authors;

        public MemoryProvider()
        {
            books = new List<Book>();
            authors = new List<Author>();

            LoadFakeData();
        }

        void LoadFakeData(){
            var authorA = new Author{Name="Author A"};
            var authorB = new Author{Name="Author B"};

            authors.Add(authorA);
            authors.Add(authorB);

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
            // If the author/s are new, then add them to the list of authors
            AddNewAuthors(book.Authors);

            books.Add(book);

            return book;
        }

        public Book UpdateBook(string id, Book book){
            var current = books.Single(x=>x.Id.Equals(id));
            // If the author/s are new, then add them to the list of authors
            AddNewAuthors(current.Authors);

            return current.MapFrom(book);
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

        public IList<Book> SearchBook(string query){
            return books.Where(x=>x.Title.Contains(query)).ToList();
        }

        public Author InsertAuthor(Author author){
            authors.Add(author);

            return author;
        }

        public Author UpdateAuthor(string id, Author author){
            var current = authors.Single(x=>x.Id.Equals(id));

            return current.MapFrom(author);
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
        public IList<Author> SearchAuthor(string query){
            return authors.Where(x=>x.Name.Contains(query)).ToList();
        }

        private void AddNewAuthors(IList<Author> authors){
            foreach (var author in authors)
            {
                if(!authors.Any(x=>x.Id.Equals(author.Id))){
                    InsertAuthor(author);
                }
            }
        }
    }
}