using System.Collections.Generic;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.Repositories
{
    public class BookRepository:IDataRepository<Book>{
        private readonly IStorageProvider storageProvider;

        public BookRepository(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public Book Get(string id){
            return storageProvider.GetBook(id);
        }

        public IList<Book> Get(){
            return storageProvider.GetBooks();
        }

        public void Insert(Book item){
            storageProvider.InsertBook(item);
        }

        public void Update(string id, Book item){
            storageProvider.UpdateBook(id, item);
        }

        public void Delete(string id){
            storageProvider.DeleteBook(id);
        }
    }
}