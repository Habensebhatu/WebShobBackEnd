using System;
using System.ComponentModel.DataAnnotations;

namespace Data_layer.Context
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal AmountTotal { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

}

