using System.Collections.Generic;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.Repositories
{
    public class AuthorRepository:IDataRepository<Author>{
        private readonly IStorageProvider storageProvider;

        public AuthorRepository(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public Author Get(string id){
            return storageProvider.GetAuthor(id);
        }

        public IList<Author> Get(){
            return storageProvider.GetAuthors();
        }

        public void Insert(Author item){
            storageProvider.InsertAuthor(item);
        }

        public void Update(string id, Author item){
            storageProvider.UpdateAuthor(id, item);
        }

        public void Delete(string id){
            storageProvider.DeleteAuthor(id);
        }
    }
}