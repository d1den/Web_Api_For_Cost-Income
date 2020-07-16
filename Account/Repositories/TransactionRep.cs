using Account.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Account.Repositories
{
    /// <summary>
    /// Репозиторий для работы с Транзакциями
    /// </summary>
    public class TransactionRep
    {
        TrContext context = new TrContext();

        /// <summary>
        /// Метод для проверки на существовании транзакции в БД по Id
        /// </summary>
        /// <param name="transaction">Принимает польцовательский объект транзакции</param>
        /// <returns>Результат проверки</returns>
        public bool TransactionExist(UserTransaction transaction)
        {
            return context.Transactions.Any(t => t.Id == transaction.Id);
        }

        /// <summary>
        /// Метод для получения одной транзакции
        /// </summary>
        /// <param name="id">Получает Id транзакции</param>
        /// <returns>Возвращает объект пользовательской транзакции</returns>
        public IQueryable<UserTransaction> GetOne(int id)
        {
            IQueryable<UserTransaction> result = GetAll().Where(r => r.Id == id);
            return result;
        }

        /// <summary>
        /// Метод для добавления новой пользовательской транзакции
        /// </summary>
        /// <param name="transaction">Принимает объект пользовательской транзакции</param>
        public void Add(UserTransaction transaction)
        {
            // Изначально проверяем, есть ли уже такая категория
            int categoryId = FindCategoryId(transaction.Category); // Если да, то берём её Id
            if (categoryId == -1) // Если нет, то создаём её и берём её Id
            {
                context.Categories.Add(new Category { Name = transaction.Category });
                context.SaveChanges();
                categoryId = FindCategoryId(transaction.Category);
            }
            Transaction newTransaction = new Transaction // Создаём объект транзакции
            {
                Type = transaction.Type,
                CategoryId = categoryId,
                Sum = transaction.Sum,
                Comment = transaction.Comment
            };
            context.Transactions.Add(newTransaction);
            context.SaveChanges();
        }

        /// <summary>
        /// Метод для обновления пользовательской транзакции
        /// </summary>
        /// <param name="transaction">Принимает объект пользовательской транзакции</param>
        public void Update(UserTransaction transaction)
        {
            // Изначально проверяем, есть ли уже такая категория
            int categoryId = FindCategoryId(transaction.Category); // Если да, то берём её Id
            if (categoryId == -1) // Если нет, то создаём её и берём её Id
            {
                context.Categories.Add(new Category { Name = transaction.Category });
                context.SaveChanges();
                categoryId = FindCategoryId(transaction.Category);
            }
            Transaction updateTransaction = new Transaction // Создаём объект транзакции
            {
                Id = transaction.Id, // С использованием Id и всех прощих параметро
                Type = transaction.Type,
                CategoryId = categoryId,
                Sum = transaction.Sum,
                Comment = transaction.Comment,
                Date = transaction.Date
            };
            context.Transactions.Update(updateTransaction); // Обновляем таблицу
            context.SaveChanges();
        }

        /// <summary>
        /// Метод для получения отчёта расходов, сгруппированных по месяцам
        /// </summary>
        /// <returns>Возвращает список сгрупированных объектов</returns>
        public List<UserTransaction> GetCostByMonth()
        {
            List<UserTransaction> all = GetAll().Where(t => t.Type == "cost").ToList();
            return GroupByMonth(all);
        }

        /// <summary>
        /// Метод для получения отчёта доходов, сгруппированных по месяцам
        /// </summary>
        /// <returns>Возвращает список сгрупированных объектов</returns>
        public List<UserTransaction> GetIncomeByMonth()
        {
            List<UserTransaction> all = GetAll().Where(t => t.Type == "income").ToList();
            return GroupByMonth(all);
        }

        /// <summary>
        /// Метод для получения отсчёта расходов, сгрупированных по категориям за период времени
        /// </summary>
        /// <param name="periodStart"></param>
        /// <param name="periodStop"></param>
        /// <returns></returns>
        public List<UserTransaction> GetCostByCategory(string periodStart, string periodStop)
        {
            List<UserTransaction> all = GetAll().Where(t => t.Type == "cost").ToList();
            return GroupByCategory(all, periodStart, periodStop);
        }

        /// <summary>
        /// Метод для получения отсчёта расходов, сгрупированных по категориям за период времени
        /// </summary>
        /// <param name="periodStart"></param>
        /// <param name="periodStop"></param>
        /// <returns></returns>
        public List<UserTransaction> GetIncomeByCategory(string periodStart, string periodStop)
        {
            List<UserTransaction> all = GetAll().Where(t => t.Type == "income").ToList();
            return GroupByCategory(all, periodStart, periodStop);
        }

        /// <summary>
        /// Метод для группировки списка объектов транзакций по Категориям и удовлетворяющим периоду времени
        /// </summary>
        /// <param name="all">Исходный список объектов</param>
        /// <param name="periodStart">Начало временного периода</param>
        /// <param name="periodStop">Конец периода</param>
        /// <returns>Возвращает отсортированный список</returns>
        private List<UserTransaction> GroupByCategory(List<UserTransaction> all, string periodStart, string periodStop)
        {
            DateTime start = DateTime.Parse(periodStart);
            DateTime stop = DateTime.Parse(periodStop);
            List<UserTransaction> result = new List<UserTransaction>();
            for (int i = 0; i < all.Count; i++)
            {
                string category = all[i].Category;
                for (int j = 0; j < all.Count; j++)
                {
                    DateTime date = DateTime.Parse(all[j].Date);
                    int comp1 = DateTime.Compare(start, date);
                    int comp2 = DateTime.Compare(date, stop);
                    if (all[j].Category == category && comp1 <= 0 && comp2 <= 0)
                    {
                        result.Add(all[j]);
                        all.Remove(all[j]);
                        j = -1;
                        i = -1;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Метод для группировки списка объектов транзакций по Месяцу
        /// </summary>
        /// <param name="all">Получает исходный список объектов</param>
        /// <returns>Выводит список перегруппированных объектов</returns>
        private List<UserTransaction> GroupByMonth(List<UserTransaction> all)
        {
            List<UserTransaction> result = new List<UserTransaction>();
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 0; j < all.Count(); j++)
                {
                    if (int.Parse(all[j].Date.Split('.')[1]) == i)
                    {
                        result.Add(all[j]);
                        all.Remove(all[j]);
                        j = -1;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Метод для получения всех объектов транзакций в пользовательском виде
        /// </summary>
        /// <returns> Возвращает набор объектов пользовательских транзакций </returns>
        public IQueryable<UserTransaction> GetAll()
        {
            return context.Transactions.Join(context.Categories,
                t => t.CategoryId,
                c => c.Id,
                (t, c) => new UserTransaction
                {
                    Id = t.Id,
                    Type = t.Type,
                    Category = c.Name,
                    Sum = t.Sum,
                    Comment = t.Comment,
                    Date = t.Date
                });
        }

        /// <summary>
        /// Метод для нахождения Id категории по её названию
        /// </summary>
        /// <param name="nameCategory">Принимает имя категории</param>
        /// <returns>Возвращает Id категории или -1, если её не существует</returns>
        private int FindCategoryId(string nameCategory)
        {
            var category = context.Categories.FirstOrDefault(c => c.Name == nameCategory);
            if (category == null)
                return -1;
            else
                return category.Id;
        }
    }

    /// <summary>
    /// Класс для описания пользовательской транзакции
    /// </summary>
    public class UserTransaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Category { get; set; } // Категория представляет название
        public double Sum { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
    }
}
