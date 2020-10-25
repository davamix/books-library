namespace BooksLibrary.API.Data.Database.Queries
{
    public interface IQueryCommand : IQuery{
        void Execute(string query);
    }
}