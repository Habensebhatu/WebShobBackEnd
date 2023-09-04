using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using Data_layer.Context.Data;

namespace business_logic_layer
{
	public class CartBLL
	{

        private readonly CartDAL _cartDAL;

        public CartBLL()
		{
			_cartDAL = new CartDAL();
		}

        public async Task<CartModel> AddCart(CartModel cart)
        {

            CartEnityModel cartFormat = new CartEnityModel()
            {
                productId = cart.productId,
                Title = cart .Title,
                Price = cart.Price,
                Description = cart.Description,
                ImageUrl = cart.ImageUrl,
                Quantity = cart.Quantity,
                CategoryName = cart.CategoryName
                
            };
            await _cartDAL.AddProduct(cartFormat);
            return cart;
        }

        public async Task<List<CartModel>> GetCart()
        {
            var cartEntities = await _cartDAL.GetCartItems();
            return cartEntities.Select(item => new CartModel
            {
                productId = item.productId,
                Title = item.Title,
                Price = item.Price,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Quantity = item.Quantity,
                CategoryName = item.CategoryName
            }).ToList();
        }

        public async Task ClearCart()
        {
            await _cartDAL.ClearCart();
        }

        public async Task RemoveCart(Guid productId)
        {
            await _cartDAL.RemoveProduct(productId);
        }

        public async Task<CartModel> UpdateCartQuantity(Guid productId)
        {
            var updatedProduct = await _cartDAL.UpdateProductQuantity(productId);
            if (updatedProduct == null)
            {
                return null;
            }

            // Convert and return
            return new CartModel
            {
                productId = updatedProduct.productId,
                Title = updatedProduct.Title,
                Price = updatedProduct.Price,
                CategoryName = updatedProduct.CategoryName,
                ImageUrl = updatedProduct.ImageUrl,
                Description = updatedProduct.Description,
                Quantity = updatedProduct.Quantity
                // ... and so on for all properties
            };
        }


    }
}

