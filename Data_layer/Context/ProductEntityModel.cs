using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_layer.Context
{
    public class Product
    {
        [Key]
        public Guid productId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductImageEnityModel> ProductImages { get; set; }
    }

}

