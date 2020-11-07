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
        private readonly IDataRepository<Book> dataRepository;
        private readonly IDataRepository<Author> authorRepository;
        private readonly IDataRepository<Category> categoryRepository;

        public BookController(IConfiguration configuration, IDataRepository<Book> dataRepository, IDataRepository<Author> authorRepository, IDataRepository<Category> categoryRepository)
        {
            this.configuration = configuration;
            this.dataRepository = dataRepository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("{id}")]
        // https://localhost:5001/api/book/1
        public Book Get(string id)
        {
            return dataRepository.Get(id);
        }

        [HttpGet]
        [Route("[action]")]
        // https://localhost:5001/api/book/GetBooks
        public ActionResult<IList<Book>> GetBooks()
        {
            return Ok(dataRepository.Get());
        }

        [HttpGet]
        [Route("[action]")]
        // https://localhost:5001/api/book/Search
        public ActionResult<IList<Book>> Search([FromQuery] string query = "")
        {
            if (!string.IsNullOrEmpty(query))
            {
                return Ok(dataRepository.Search(query));
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
            dataRepository.Insert(book);
            
            // Return the book with Id
            return Ok(book);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<Book> UpdateBook(string id, [FromBody] Book book)
        {
            var updatedBook = dataRepository.Update(id, book);

            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(string id)
        {
            dataRepository.Delete(id);

            return Ok($"Book {id} removed");
        }
    }
}