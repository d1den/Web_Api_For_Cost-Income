using System;
using System.ComponentModel.DataAnnotations;

namespace Account.Model
{
    public class Transaction
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Type { get; set; } // Тип - доход или расход
        public int CategoryId { get; set; }
        public double Sum { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [MaxLength(20)]
        public string Date { get; set; } = DateTime.Now.ToString("d"); // Дата создаётся при создании объекта

    }
}
