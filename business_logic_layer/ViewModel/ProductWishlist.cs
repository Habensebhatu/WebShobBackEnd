using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_layer.Context.Data
{
    public class ProductWishlist
    {
        [Key]
        public Guid ProductWishlistId { get; set; }

        public Guid WishlistId { get; set; }
        public Guid ProductId { get; set; }

        [ForeignKey("WishlistId")]
        public WishlistEntityModel Wishlist { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
