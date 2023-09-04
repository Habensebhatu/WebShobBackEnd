using System;
using System.ComponentModel.DataAnnotations;

namespace Data_layer.Context
{
	public class CustomerEntityModel
	{
        [Key]
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string recipientName { get; set; }
        public string line1 { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string phoneNumber{ get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}

