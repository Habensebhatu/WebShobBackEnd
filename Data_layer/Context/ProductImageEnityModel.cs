using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_layer.Context
{
	public class ProductImageEnityModel
	{

        [Key]
        public Guid ProductImageId { get; set; }
        public string ImageUrl { get; set; }
        public int Index { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}

