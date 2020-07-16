using System.Collections.Generic;
using System.Linq;
using Account.Model;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers
{ 
    /// <summary>
    /// Класс контроллера операций с категориями
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        TrContext context = new TrContext();

        // GET: category
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return context.Categories.ToList();
        }

        // POST: category
        [HttpPost]
        public ActionResult<Category> Post(Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            context.Categories.Add(category);
            context.SaveChanges();
            return Ok(category);
        }

        // PUT: category
        [HttpPut]
        public ActionResult<Category> Put(Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            if (!context.Categories.Any(x => x.Id == category.Id))
            {
                return NotFound();
            }

            context.Update(category);
            context.SaveChanges();
            return Ok(category);
        }
    }
}
