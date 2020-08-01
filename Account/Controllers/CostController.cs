using System.Collections.Generic;
using System.Linq;
using Account.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Account.Controllers
{
    /// <summary>
    /// Класс для операций с расходами
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CostController : ControllerBase
    {
        TransactionRep repos = new TransactionRep();

        // GET: cost/bymonth
        [HttpGet("bymonth")] // Получение отчёта по расходам сгрупированного по месяцам
        public List<UserTransaction> GetByMonth()
        {
            return repos.GetCostByMonth();
        }

        // GET: cost/bycategory/{periodStart}&{periodStop}
        [HttpGet("bycategory/{id}")] // Получение отчёта по расходам сгрупированного по категориям за период времени
        public List<UserTransaction> GetByCategory(string id)
        {
            string periodStart = id.Split('&')[0];
            string periodStop = id.Split('&')[1];
            return repos.GetCostByCategory(periodStart, periodStop);
        }

        // GET: cost/{id}
        [HttpGet("{id}")] // Получение расхода по Id
        public UserTransaction GetOne(int id)
        {
            return repos.GetOneTransaction(id);
        }

        // POST: cost
        [HttpPost] // Добавление нового расхода
        public ActionResult<UserTransaction> Post(UserTransaction transaction)
        {
            transaction.Type = "cost";
            if (transaction == null)
            {
                return BadRequest();
            }
            repos.AddTransaction(transaction);
            return Ok(repos.GetLastCost());
        }

        // PUT: cost
        [HttpPut] // Изменение расхода
        public ActionResult<UserTransaction> Put(UserTransaction transaction)
        {
            transaction.Type = "cost";
            if (transaction == null)
            {
                return BadRequest();
            }
            if (!repos.TransactionExist(transaction))
            {
                return NotFound();
            }
            repos.UpdateTransaction(transaction);
            return Ok(transaction);
        }
    }
}
