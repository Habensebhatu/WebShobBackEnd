using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using Data_layer.Context.Data;

namespace business_logic_layer
{
	public class WishlistBLL
	{
        private readonly WishlistDAL _wishlistDAL;
        public WishlistBLL()
		{
            _wishlistDAL = new WishlistDAL();
		}

        public async Task <WishlistModel> AddProductToWishlist(Guid productId, string userID)
        {
            if (!Guid.TryParse(userID, out var parsedUserId))
            {
                throw new ArgumentException("Invalid userId");
            }

            var userWishlist = _wishlistDAL.GetWishlistByUserId(parsedUserId);

            if (userWishlist == null)
            {
                // Create a new wishlist for the user.
                userWishlist = await _wishlistDAL.CreateWishlistForUser(parsedUserId);
               
            }
            Console.WriteLine($"userId is gevonden : {productId}");
            var productWishlistItem = new ProductWishlist
            {
                ProductWishlistId = Guid.NewGuid(),
                WishlistId = userWishlist.WishlistId,
                ProductId = productId
            };
            await _wishlistDAL.AddProductToWishlist(productWishlistItem);
            WishlistModel formaat = new WishlistModel()
            {
                UserId = parsedUserId,
                WishlistId = userWishlist.WishlistId
            };
            return formaat;
        }

        public async Task<List<productModelS>> GetWishlistProducts(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                throw new ArgumentException("Invalid userId");
            }

            var products = await _wishlistDAL.GetProductsInWishlist(parsedUserId);

            return products.Select(p => new productModelS
            {
                productId = p.productId,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                ImageUrls = p.ProductImages
                    .OrderBy(pi => pi.Index)  
                    .Select(pi => new ImageUpdateModel
                    {
                        Index = pi.Index,
                        File = pi.ImageUrl 
                    }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
        }

        public async Task<bool> DeleteProductFromWishlist(Guid productId, string userId)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                throw new ArgumentException("Invalid userId");
            }

            return await _wishlistDAL.DeleteProductFromWishlist(productId, parsedUserId);
        }


    }
}

