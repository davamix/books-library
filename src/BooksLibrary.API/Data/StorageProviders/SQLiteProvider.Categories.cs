using System;
using System.Collections.Generic;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.StorageProviders
{
    public partial class SQLiteProvider : IStorageProvider
    {
        public Category GetCategory(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Category> GetCategories()
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
                        Id = reader.GetString(0),
                        Name = reader.GetString(1)
                    });
                }

                return categories;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }
        
        public Category InsertCategory(Category category)
        {
            // QUERY
            var query = $@"INSERT INTO authors(id, name) VALUES('{category.Id}', '{category.Name}')";

            //EXEXUTE
            try{
                queryCommand.Execute(query);
            }catch(SqliteException){
                throw;
            }

            return category;
            
        }

        public Category UpdateCategory(string id, Category author)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCategory(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Category> SearchCategory(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}