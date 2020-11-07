using System.Collections.Generic;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Data.StorageProviders
{
    public interface IStorageProvider<T> where T:EntityBase<T>
    {
        void Insert(T book);
        T Update(string id, T book);
        void Delete(string id);
        IList<T> Get();
        T Get(string id);
        IList<T> Search(string query);
    }
}