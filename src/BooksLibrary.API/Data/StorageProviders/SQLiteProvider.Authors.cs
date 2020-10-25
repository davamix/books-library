using System;
using System.Collections.Generic;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;

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
                        Id = reader.GetString(0),
                        Name = reader.GetString(1)
                    });
                }

                return authors;
            };

            // EXECUTE
            return queryReader.Execute(query, mapper);
        }
        
        public Author InsertAuthor(Author author)
        {
            throw new System.NotImplementedException();
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