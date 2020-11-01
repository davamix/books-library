using System;
using System.Collections.Generic;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;
using BooksLibrary.API.Data.Database.Extensions;

namespace BooksLibrary.API.Data.StorageProviders
{
    public partial class SQLiteProvider : IStorageProvider
    {
        public Author GetAuthor(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Author> GetAuthors()
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
        
        public Author InsertAuthor(Author author)
        {
            // QUERY
            var query = $@"INSERT INTO authors(id, name) VALUES('{author.Id}', '{author.Name}')";

            //EXEXUTE
            try{
                queryCommand.Execute(query);
            }catch(SqliteException){
                throw;
            }

            return author;
            
        }

        public Author UpdateAuthor(string id, Author author)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAuthor(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Author> SearchAuthor(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}