using System;
using Microsoft.EntityFrameworkCore;


namespace Account.Model
{
    /// <summary>
    /// Класс контекста, описывающий БД
    /// </summary>
    public class TrContext : DbContext
    {
        public string NameDB { get; set; } = "TrDB"; // Название файла с базой данных
        public TrContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(String.Format("Filename={0}.db", NameDB));
        }
        public DbSet<Category> Categories { get; set; } // Таблица категории
        public DbSet<Transaction> Transactions { get; set; } // Таблица транзакции
    }
}
