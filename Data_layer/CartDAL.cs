using System;
using Data_layer.Context;
using Data_layer.Context.Data;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
	public class CartDAL
	{
        private readonly MyDbContext _context;
        public CartDAL()
		{
            _context = new MyDbContext();
        }

        public async Task<CartEnityModel> AddProduct(CartEnityModel cartItem)
        {
            var existingItem = _context.Cart.FirstOrDefault(item => item.productId == cartItem.productId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
                _context.Cart.Update(existingItem);
            }
            else
            {
                _context.Cart.Add(cartItem);
            }
            await _context.SaveChangesAsync();

            return cartItem;  // Return the added or updated cart item
        }


        public async Task<List<CartEnityModel>> GetCartItems(string sessionId)
        {
            return await _context.Cart.Where(item => item.SessionId == sessionId).ToListAsync();
        }


        public async Task ClearCart(string sessionId)
        {
            var RemoveCart = await _context.Cart.Where(item => item.SessionId == sessionId).ToListAsync();
            _context.Cart.RemoveRange(RemoveCart);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProduct(Guid productId, string sessionId)
        {
            Console.WriteLine($"sessionId: {sessionId}");
            var productToRemove = _context.Cart.FirstOrDefault(item => item.productId == productId && item.SessionId == sessionId);

            if (productToRemove != null)
            {
                _context.Cart.Remove(productToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CartEnityModel> UpdateProductQuantity(Guid productId, string sessionId)
        {
            var productToUpdate = _context.Cart.FirstOrDefault(item => item.productId == productId && item.SessionId == sessionId);

            if (productToUpdate != null)
            {
                productToUpdate.Quantity += -1;

                if (productToUpdate.Quantity <= 0)
                {
                    _context.Cart.Remove(productToUpdate);
                }
                else
                {
                    _context.Cart.Update(productToUpdate);
                }

                await _context.SaveChangesAsync();
            }

            return productToUpdate;  // Return the updated or removed product
        }



    }
}

