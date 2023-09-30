using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_layer.Context.Data
{
    public class WishlistEntityModel
    {
        [Key]
        public Guid WishlistId { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public UserRegistrationEntityModel User { get; set; }
        public virtual ICollection<ProductWishlist> ProductWishlists { get; set; }
    }
}
