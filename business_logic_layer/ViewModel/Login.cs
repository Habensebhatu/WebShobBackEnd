using System;
using System.ComponentModel.DataAnnotations;

namespace business_logic_layer.ViewModel
{
	public class Login
	{
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Optionally, if you have a "Remember Me" functionality:
        public bool RememberMe { get; set; }
    }
}

