using System.Collections.Generic;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Data.Repositories
{
    public interface IDataRepository<T> where T: EntityBase<T>{
        T Get(string id);
        IList<T> Get();
        void Insert(T item);
        void Update(string id, T item);
        void Delete(string id);
    }
}