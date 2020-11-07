using System;
using System.Collections.Generic;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;
using BooksLibrary.API.Data.Database.Extensions;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.Database.Queries;

namespace BooksLibrary.API.Data.StorageProviders.SQLiteProvider
{
    public class AuthorStorageProvider : SQLiteProviderBase<Author>
    {
        public AuthorStorageProvider(IDatabaseConfiguration databaseConfiguration, IQueryReader queryReader, IQueryCommand queryCommand)
            :base(databaseConfiguration, queryReader, queryCommand)
        {}
        
        public override Author Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public override IList<Author> Get()
        {
            // QUERY
            var query = @"SELECT id, name FROM authors;";

            // MAPPER
            Func<SqliteDataReader, List<Author>> mapper = (SqliteDataReader reader) =>
            {
                var authors = new List<Author>();

                while (reader.Read())
                {
                    authors.Add(new Author
                    {
                        Id = reader.GetValue<string>(0),
                        Name = reader.GetValue<string>(1)
                    });
                }

                return authors;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }
        
        protected override void InsertImp(Author author)
        {
            // QUERY
            var query = $@"INSERT INTO authors(id, name) VALUES('{author.Id}', '{author.Name}')";

            //EXEXUTE
            try{
                queryCommand.Execute(query);
            }catch(SqliteException){
                throw;
            }
        }

        public override Author Update(string id, Author author)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public override IList<Author> Search(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}