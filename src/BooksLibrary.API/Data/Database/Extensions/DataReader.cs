using System;
using Microsoft.Data.Sqlite;

namespace BooksLibrary.API.Data.Database.Extensions
{
    public static class DataReader
    {
        public static T GetValue<T>(this SqliteDataReader reader, int index, bool allowNull = false)
        {
            var value = reader[index];

            if (value.GetType() == typeof(DBNull))
            {
                if(allowNull) 
                    return default(T);
                else 
                    throw new SqliteException($"Index {index} cannot be null", 0); 
            }

            return (T)value;
        }
    }
}