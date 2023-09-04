using System;
using Data_layer.Context;
using System.ComponentModel.DataAnnotations;

namespace business_logic_layer.ViewModel
{
	public class OrderDetailModel
	{
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal AmountTotal { get; set; }
    }
}

