using System;
using Data_layer.Context;
using System.ComponentModel.DataAnnotations;

namespace business_logic_layer.ViewModel
{
	public class CategoryModel
	{
        public Guid categoryId { get; set; }
        public string Name { get; set; }
        public int quantityProduct { get; set; }

    }
}

