﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data_layer.Context.Data;

namespace Data_layer.Context
{
    public class Product
    {
        [Key]
        public Guid productId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal? Kilo { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; } = false;
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductImageEnityModel> ProductImages { get; set; }
        public virtual ICollection<ProductWishlist> ProductWishlists { get; set; }

    }

}

