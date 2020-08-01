using System.Collections.Generic;
using System.Linq;
using Account.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers
{
    /// <summary>
    /// Класс контроллера для операций с доходами
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        TransactionRep repos = new TransactionRep();

        // GET: income/bymonth
        [HttpGet("bymonth")] // Получение отчёта по доходам сгрупированного по месяцам
        public List<UserTransaction> GetByMonth()
        {
            return repos.GetIncomeByMonth();
        }

        // GET: income/bycategory/{periodStart}&{periodStop}
        [HttpGet("bycategory/{id}")] // Получение доходам по расходам сгрупированного по категориям за период времени
        public List<UserTransaction> GetByCategory(string id)
        {
            string periodStart = id.Split('&')[0];
            string periodStop = id.Split('&')[1];
            return repos.GetIncomeByCategoryAndPeriod(periodStart, periodStop);
        }

        // GET: income/{id}
        [HttpGet("{id}")] // Получение дохода по Id
        public UserTransaction GetOne(int id)
        {
            return repos.GetOneTransaction(id);
        }

        // POST: income
        [HttpPost] // Добавление нового дохода
        public ActionResult<UserTransaction> Post(UserTransaction transaction)
        {
            transaction.Type = "income";
            if (transaction == null)
            {
                return BadRequest();
            }
            repos.AddTransaction(transaction);
            return Ok(repos.GetLastIncome());
        }

        // PUT: income
        [HttpPut] // Изменение дохода
        public ActionResult<UserTransaction> Put(UserTransaction transaction)
        {
            transaction.Type = "income";
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
