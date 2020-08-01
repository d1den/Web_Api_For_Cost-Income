using Account.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Account.Repositories
{
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


        public UserTransaction GetOneTransaction(int id)
        {
            UserTransaction result = GetAllTransactions().Where(r => r.Id == id).FirstOrDefault();
            return result;
        }

        private UserTransaction GetLastTransaction(string type)
        {
            int lastTransaction = context.Transactions.Where(t => t.Type == type).Max(c => c.Id);
            UserTransaction newCategory = GetOneTransaction(lastTransaction);
            return newCategory;
        }

        public UserTransaction GetLastIncome()
        {
            return GetLastTransaction("income");
        }

        public UserTransaction GetLastCost()
        {
            return GetLastTransaction("cost");
        }

        public void AddTransaction(UserTransaction transaction)
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

        public void UpdateTransaction(UserTransaction transaction)
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

        public List<UserTransaction> GetCostByMonth()
        {
            List<UserTransaction> all = GetAllTransactions().Where(t => t.Type == "cost").ToList();
            return GroupByMonth(all);
        }
        public List<UserTransaction> GetIncomeByMonth()
        {
            List<UserTransaction> all = GetAllTransactions().Where(t => t.Type == "income").ToList();
            return GroupByMonth(all);
        }
        public List<UserTransaction> GetCostByCategory(string periodStart, string periodStop)
        {
            List<UserTransaction> all = GetAllTransactions().Where(t => t.Type == "cost").ToList();
            return GroupByCategoryAndPeriod(all, periodStart, periodStop);
        }

        public List<UserTransaction> GetIncomeByCategoryAndPeriod(string periodStart, string periodStop)
        {
            List<UserTransaction> all = GetAllTransactions().Where(t => t.Type == "income").ToList();
            return GroupByCategoryAndPeriod(all, periodStart, periodStop);
        }

        private List<UserTransaction> GroupByCategoryAndPeriod(List<UserTransaction> allTransactions, string periodStart, string periodStop)
        {
            DateTime start = DateTime.Parse(periodStart);
            DateTime stop = DateTime.Parse(periodStop);
            List<UserTransaction> result = new List<UserTransaction>();
            for (int i = 0; i < allTransactions.Count; i++)
            {
                string category = allTransactions[i].Category;
                for (int j = 0; j < allTransactions.Count; j++)
                {
                    DateTime date = DateTime.Parse(allTransactions[j].Date);
                    int comp1 = DateTime.Compare(start, date);
                    int comp2 = DateTime.Compare(date, stop);
                    if (allTransactions[j].Category == category && comp1 <= 0 && comp2 <= 0)
                    {
                        result.Add(allTransactions[j]);
                        allTransactions.Remove(allTransactions[j]);
                        j = -1;
                        i = -1;
                    }
                }
            }
            return result;
        }

        private List<UserTransaction> GroupByMonth(List<UserTransaction> allTransactions)
        {
            List<UserTransaction> result = new List<UserTransaction>();
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 0; j < allTransactions.Count(); j++)
                {
                    if (int.Parse(allTransactions[j].Date.Split('.')[1]) == i)
                    {
                        result.Add(allTransactions[j]);
                        allTransactions.Remove(allTransactions[j]);
                        j = -1;
                    }
                }
            }
            return result;
        }

        public IQueryable<UserTransaction> GetAllTransactions()
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

        private int FindCategoryId(string nameCategory)
        {
            var category = context.Categories.FirstOrDefault(c => c.Name == nameCategory);
            if (category == null)
                return -1;
            else
                return category.Id;
        }
    }

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
