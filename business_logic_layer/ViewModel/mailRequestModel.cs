using System;
namespace business_logic_layer.ViewModel
{
	public class mailRequestModel
	{
        public string recipientName;
        public string CustomerName;
        public string city;
        public string line1;
        public string postalCode;
        public DateTime OrderDate { get; set; }
        public long OrderNummer { get; set; }
        public string paymentMethodType;
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }

    public class OrderItemModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string ImageUrl { get; set; }
    }


}

                            