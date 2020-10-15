using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BooksLibrary.API.Data.Repositories;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IDataRepository<Book> dataRepository;

        public BookController(IDataRepository<Book> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        [HttpGet]
        [Route("{id}")]
        // https://localhost:5001/api/book/1
        public Book Get(string id){
            return dataRepository.Get(id);
        }

        [HttpGet]
        [Route("[action]")]
        // https://localhost:5001/api/book/GetBooks
        public IList<Book> GetBooks(){
            return dataRepository.Get();
        }

        [HttpPost]
        public ActionResult<Book> AddBook([FromBody] Book book){
            dataRepository.Insert(book);
            // Return the book with Id
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<Book> UpdateBook(string id, [FromBody] Book book){
            dataRepository.Update(id, book);

            return Ok();
        }

        [HttpDelete]
        public ActionResult DeleteBook(string id){
            dataRepository.Delete(id);

            return Ok($"Book {id} removed");
        }
    }
}