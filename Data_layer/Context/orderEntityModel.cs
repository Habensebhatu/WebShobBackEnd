using System;
using System.ComponentModel.DataAnnotations;

namespace Data_layer.Context
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public long OrderNumber { get; set; }
        public CustomerEntityModel Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

}

