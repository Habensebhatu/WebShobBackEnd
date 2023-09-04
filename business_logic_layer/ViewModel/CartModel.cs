﻿using System;
namespace business_logic_layer.ViewModel
{
	public class CartModel
	{
        public Guid productId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
    }
}

