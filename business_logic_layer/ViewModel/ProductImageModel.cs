using System;
using Data_layer.Context;
using System.ComponentModel.DataAnnotations.Schema;

namespace business_logic_layer.ViewModel
{
	public class ProductImageModel
	{
        public Guid ProductImageId { get; set; }
        public string ImageUrl { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}

