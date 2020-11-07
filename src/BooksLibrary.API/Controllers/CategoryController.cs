using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BooksLibrary.API.Data.Repositories;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IDataRepository<Category> dataRepository;

        public CategoryController(IDataRepository<Category> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        [HttpGet]
        [Route("{id}")]
        // https://localhost:5001/api/category/1
        public Category Get(string id){
            return dataRepository.Get(id);
        }

        [HttpGet]
        [Route("[action]")]
        // https://localhost:5001/api/category/GetCategories
        public ActionResult<IList<Category>> GetCategories(){
            return Ok(dataRepository.Get());
        }

        [HttpPost]
        // https://localhost:5001/api/category
        public ActionResult<Category> AddCategory([FromBody] Category category){
            dataRepository.Insert(category);
            // Return the book with Id
            return Ok(category);
        }
    }
}