using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BooksLibrary.API.Data.Repositories;
using BooksLibrary.API.Entities;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace BooksLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IBookRepository bookRepository;
        private readonly IDataRepository<Author> authorRepository;
        private readonly IDataRepository<Category> categoryRepository;

        public BookController(IConfiguration configuration, IBookRepository bookRepository, IDataRepository<Author> authorRepository, IDataRepository<Category> categoryRepository)
        {
            this.configuration = configuration;
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("{id}")]
        // https://localhost:5001/api/book/1
        public Book Get(string id)
        {
            return bookRepository.Get(id);
        }

        [HttpGet]
        [Route("[action]")]
        // https://localhost:5001/api/book/GetBooks
        public ActionResult<IList<Book>> GetBooks()
        {
            return Ok(bookRepository.Get());
        }

        [HttpGet]
        [Route("[action]")]
        // https://localhost:5001/api/book/Search
        public ActionResult<IList<Book>> Search([FromQuery] string query = "")
        {
            if (!string.IsNullOrEmpty(query))
            {
                return Ok(bookRepository.Search(query));
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Book> AddBook([FromBody] Book book)
        {
            // Save new authors
            foreach (var author in book.Authors)
            {
                if (string.IsNullOrEmpty(author.Id))
                {
                    // Save author
                    authorRepository.Insert(author);
                }
            }

            // Save new categories
            foreach (var category in book.Categories)
            {
                if (string.IsNullOrEmpty(category.Id))
                {
                    // Save category
                    categoryRepository.Insert(category);
                }
            }

            // Save book
            bookRepository.Insert(book);
            
            // Return the book with Id
            return Ok(book);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<Book> UpdateBook(string id, [FromBody] Book book)
        {
            // Save new authors
            foreach (var author in book.Authors)
            {
                if (string.IsNullOrEmpty(author.Id))
                {
                    // Save author
                    authorRepository.Insert(author);
                }
            }

            // Save new categories
            foreach (var category in book.Categories)
            {
                if (string.IsNullOrEmpty(category.Id))
                {
                    // Save category
                    categoryRepository.Insert(category);
                }
            }

            var updatedBook = bookRepository.Update(id, book);

            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(string id)
        {
            bookRepository.Delete(id);

            return Ok($"Book {id} removed");
        }

        [HttpDelete]
        [Route("{book_id}/category/{category_id}")]
        public ActionResult DeleteCategory(string book_id, string category_id){
            bookRepository.DeleteBookCategory(book_id, category_id);
            
            return Ok();
        }

        [HttpDelete]
        [Route("{book_id}/author/{author_id}")]
        public ActionResult DeleteAuthor(string book_id, string author_id){
            bookRepository.DeleteBookAuthor(book_id, author_id);

            return Ok();
        }
    }
}