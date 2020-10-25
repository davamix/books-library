using System;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.Database.Queries
{
    public interface IQueryReader : IQuery{
        T Execute<T>(string query, Func<SqliteDataReader, T> mapper);
    }
}