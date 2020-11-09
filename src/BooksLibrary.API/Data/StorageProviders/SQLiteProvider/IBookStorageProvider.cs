using System;
using System.Collections.Generic;
using BooksLibrary.API.Entities;
using Microsoft.Data.Sqlite;
using BooksLibrary.API.Data.Database.Extensions;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Data.Database.Queries;

namespace BooksLibrary.API.Data.StorageProviders.SQLiteProvider
{
    public interface IBookStorageProvider : IStorageProvider<Book>
    {
        void DeleteBookCategory(string bookId, string categoryId);
        void DeleteBookAuthor(string bookId, string authorId);
    }
}