using System;
using Data_layer.Context;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace business_logic_layer.ViewModel
{
	public class productModel
	{
        public Guid productId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal? Kilo { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsPopular { get; set; } = false;
        public List<IFormFile> NewImages { get; set; }
        public List<ExistingImageUrlModel> ExistingImageUrls { get; set; }
        public List<int> NewImageIndices { get; set; }

    }

    public class ExistingImageUrlModel
    {
        public int index { get; set; }
        public string file { get; set; }
    }



}

