using System.Collections.Generic;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.Repositories
{
    public class AuthorRepository : IDataRepository<Author>
    {
        private readonly IStorageProvider<Author> storageProvider;

        public AuthorRepository(IStorageProvider<Author> storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public Author Get(string id)
        {
            return storageProvider.Get(id);
        }

        public IList<Author> Get()
        {
            return storageProvider.Get();
        }

        public void Insert(Author item)
        {
            storageProvider.Insert(item);
        }

        public Author Update(string id, Author item)
        {
            return storageProvider.Update(id, item);
        }

        public void Delete(string id)
        {
            storageProvider.Delete(id);
        }

        public IList<Author> Search(string query)
        {
            return storageProvider.Search(query);
        }
    }
}