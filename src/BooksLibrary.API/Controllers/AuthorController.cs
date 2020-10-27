using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BooksLibrary.API.Data.Repositories;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IDataRepository<Author> dataRepository;

        public AuthorController(IDataRepository<Author> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        [HttpGet]
        [Route("{id}")]
        // https://localhost:5001/api/author/1
        public Author Get(string id){
            return dataRepository.Get(id);
        }

        [HttpGet]
        [Route("[action]")]
        // https://localhost:5001/api/author/GetAuthors
        public ActionResult<IList<Author>> GetAuthors(){
            return Ok(dataRepository.Get());
        }

        [HttpPost]
        // https://localhost:5001/api/author
        public ActionResult<Author> AddAuthor([FromBody] Author author){
            var newAuthor = dataRepository.Insert(author);
            // Return the book with Id
            return Ok(newAuthor);
        }
    }
}