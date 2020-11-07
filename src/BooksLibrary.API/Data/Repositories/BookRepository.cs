using System.Collections.Generic;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.Repositories
{
    public class BookRepository:IDataRepository<Book>{
        private readonly IStorageProvider<Book> storageProvider;

        public BookRepository(IStorageProvider<Book> storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public Book Get(string id){
            return storageProvider.Get(id);
        }

        public IList<Book> Get(){
            return storageProvider.Get();
        }

        public void Insert(Book item){
            storageProvider.Insert(item);
        }

        public Book Update(string id, Book item){
            return storageProvider.Update(id, item);
        }

        public void Delete(string id){
            storageProvider.Delete(id);
        }

        public IList<Book> Search(string query){
            return storageProvider.Search(query);
        }
    }
}