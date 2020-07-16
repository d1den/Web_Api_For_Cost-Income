using System.ComponentModel.DataAnnotations;

namespace Account.Model
{
    /// <summary>
    /// Модель, описывающая таблицу Категории 
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }
}
