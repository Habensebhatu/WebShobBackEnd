using System;
using System.ComponentModel.DataAnnotations;

namespace Data_layer.Context
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public int quantityProduct { get; set; }
        public ICollection<Product> Products { get; set; }
    }

}

