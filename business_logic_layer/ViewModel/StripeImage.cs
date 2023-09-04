using System;
namespace business_logic_layer.ViewModel
{
	public class StripeImage
	{
        public Guid productId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrls { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
    }
}

