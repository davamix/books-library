using System.Collections.Generic;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.StorageProviders;

namespace BooksLibrary.API.Data.Repositories
{
    public class CategoryRepository : IDataRepository<Category>
    {
        private readonly IStorageProvider storageProvider;

        public CategoryRepository(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public Category Get(string id)
        {
            return storageProvider.GetCategory(id);
        }

        public IList<Category> Get()
        {
            return storageProvider.GetCategories();
        }

        public Category Insert(Category item)
        {
            var category = storageProvider.InsertCategory(item);

            return category;
        }

        public Category Update(string id, Category item)
        {
            return storageProvider.UpdateCategory(id, item);
        }

        public void Delete(string id)
        {
            storageProvider.DeleteCategory(id);
        }

        public IList<Category> Search(string query)
        {
            return storageProvider.SearchCategory(query);
        }
    }
}