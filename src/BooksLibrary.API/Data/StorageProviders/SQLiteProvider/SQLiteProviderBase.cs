using System;
using System.Collections.Generic;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.Database.Queries;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Data.StorageProviders.SQLiteProvider
{
    public abstract class SQLiteProviderBase<T> : IStorageProvider<T> where T:EntityBase<T>
    {
        protected readonly IDatabaseConfiguration databaseConfiguration;
        protected readonly IQueryReader queryReader;
        protected readonly IQueryCommand queryCommand;

        public SQLiteProviderBase(IDatabaseConfiguration databaseConfiguration, IQueryReader queryReader, IQueryCommand queryCommand)
        {
            this.databaseConfiguration = databaseConfiguration;
            this.queryReader = queryReader;
            this.queryCommand = queryCommand;
        }

        public virtual void Insert(T entity){
            entity.Id = GenerateId();

            InsertImp(entity);
        }
        protected abstract void InsertImp(T entity);
        public abstract T Update(string id, T entity);
        public abstract void Delete(string id);
        public abstract IList<T> Get();
        public abstract T Get(string id);
        public abstract IList<T> Search(string query);

        private string GenerateId() => Guid.NewGuid().ToString();
    }
}