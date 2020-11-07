using System.Collections.Generic;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.Repositories
{
    public class CategoryRepository : IDataRepository<Category>
    {
        private readonly IStorageProvider<Category> storageProvider;

        public CategoryRepository(IStorageProvider<Category> storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public Category Get(string id)
        {
            return storageProvider.Get(id);
        }

        public IList<Category> Get()
        {
            return storageProvider.Get();
        }

        public void Insert(Category item)
        {
            storageProvider.Insert(item);
        }

        public Category Update(string id, Category item)
        {
            return storageProvider.Update(id, item);
        }

        public void Delete(string id)
        {
            storageProvider.Delete(id);
        }

        public IList<Category> Search(string query)
        {
            return storageProvider.Search(query);
        }
    }
}