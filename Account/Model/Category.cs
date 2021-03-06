﻿using System.ComponentModel.DataAnnotations;

namespace Account.Model
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }
}
