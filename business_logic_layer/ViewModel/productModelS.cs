using System;

namespace business_logic_layer.ViewModel
{
	public class productModelS
	{
        public Guid productId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<ImageUpdateModel> ImageUrls { get; set; }
        public string CategoryName { get; set; }
        public bool IsPopular { get; set; } = false;
        public Guid CategoryId { get; set; }
    }
}

