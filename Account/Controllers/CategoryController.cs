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

        [HttpGet("{id}")]
        public Category GetOne(int id)
        {
            return context.Categories.Find(id);
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
            int idNewCategory = context.Categories.Max(c => c.Id);
            Category newCategory = context.Categories.Find(idNewCategory);
            return Ok(newCategory);
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
