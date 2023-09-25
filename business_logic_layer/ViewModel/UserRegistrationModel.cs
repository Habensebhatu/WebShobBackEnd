using System;
using System.ComponentModel.DataAnnotations;

namespace business_logic_layer.ViewModel
{
	public class UserRegistrationModel
	{

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            public Addres Address { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            public Gender Gender { get; set; }

    }

        public class Addres
        {
            [Required]
            public string Street { get; set; }

            public int? Number { get; set; } // Assuming house number, can be null

            [Required]
            public string ZipCode { get; set; }

            [Required]
            public string Residence { get; set; }
        }

        public enum Gender
        {
            Male,
            Female,
            Other,
            PreferNotToSay
        }
 }



