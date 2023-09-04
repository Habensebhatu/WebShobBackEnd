using System;
using System.ComponentModel.DataAnnotations;

namespace Data_layer.Context
{
	public class LoginEnitiyModel
	{
        [Key]
        public int LoginId { get; set; }
        public string useName { get; set; }
        public string password { get; set; }  
	}
}

