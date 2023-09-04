using System;
using Data_layer.Context;
using System.ComponentModel.DataAnnotations;

namespace business_logic_layer.ViewModel
{
	public class OrderModel
	{
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string recipientName { get; set; }
        public long OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderDetailModel> OrderDetails { get; set; }

    }

    public class GetOrderModel
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string recipientName { get; set; }
        public string line1 { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string phoneNumber { get; set; }
        public long OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderDetailModel> OrderDetails { get; set; }
    }
}

