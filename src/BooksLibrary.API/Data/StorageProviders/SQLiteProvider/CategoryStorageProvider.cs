using System;
using System.Collections.Generic;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;
using BooksLibrary.API.Data.Database.Extensions;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.Database.Queries;

namespace BooksLibrary.API.Data.StorageProviders.SQLiteProvider
{
    public class CategoryStorageProvider : SQLiteProviderBase<Category>
    {
        public CategoryStorageProvider(IDatabaseConfiguration databaseConfiguration, IQueryReader queryReader, IQueryCommand queryCommand)
            :base(databaseConfiguration, queryReader, queryCommand)
        {}

        public override Category Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public override IList<Category> Get()
        {
            // QUERY
            var query = @"SELECT id, name FROM categories;";

            // MAPPER
            Func<SqliteDataReader, List<Category>> mapper = (SqliteDataReader reader) =>
            {
                var categories = new List<Category>();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Id = reader.GetValue<string>(0),
                        Name = reader.GetValue<string>(1)
                    });
                }

                return categories;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }
        
        protected override void InsertImp(Category category)
        {
            // QUERY
            var query = $@"INSERT INTO categories(id, name) VALUES('{category.Id}', '{category.Name}')";

            //EXEXUTE
            try{
                queryCommand.Execute(query);
            }catch(SqliteException){
                throw;
            }
        }

        public override Category Update(string id, Category category)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public override IList<Category> Search(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}