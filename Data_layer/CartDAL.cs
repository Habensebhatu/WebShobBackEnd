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

            return cartItem; 
        }

        public Guid GetDefaultUserId()
        {
            return _context.UserRegistration.FirstOrDefault(u => u.Email == "default@guest.com").UserId;
        }


        public async Task<List<CartEnityModel>> GetCartItems(string sessionId, Guid? userId = null)
        {
            
            if (userId.HasValue && userId.Value != Guid.Empty)
                return await _context.Cart.Where(item => item.UserId == userId.Value).ToListAsync();
            else
                return await _context.Cart.Where(item => item.SessionId == sessionId).ToListAsync();
        }

        public async Task ClearCart(string sessionId, Guid? userId)
        {
            Console.WriteLine($"sessionId : {sessionId}");
            List<CartEnityModel> RemoveCart;

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                RemoveCart = await _context.Cart.Where(item => item.UserId == userId.Value).ToListAsync();
            }
            else
            {
                RemoveCart = await _context.Cart.Where(item => item.SessionId == sessionId).ToListAsync();
            }

            Console.WriteLine($"RemoveCart: {RemoveCart.Count} items to remove");
            _context.Cart.RemoveRange(RemoveCart);
            await _context.SaveChangesAsync();
        }



        public async Task RemoveProduct(Guid productId, Guid? userId, string sessionId)
        {
            CartEnityModel productToRemove = null;

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                productToRemove = _context.Cart.FirstOrDefault(item => item.productId == productId && item.UserId == userId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(sessionId))
            {
                productToRemove = _context.Cart.FirstOrDefault(item => item.productId == productId && item.SessionId == sessionId);
            }

            if (productToRemove != null)
            {
                _context.Cart.Remove(productToRemove);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<CartEnityModel> UpdateProductQuantity(Guid productId, Guid? userId, string sessionId)
        {
            CartEnityModel productToUpdate = null;

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                productToUpdate = _context.Cart.FirstOrDefault(item => item.productId == productId && item.UserId == userId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(sessionId))
            {
                productToUpdate = _context.Cart.FirstOrDefault(item => item.productId == productId && item.SessionId == sessionId);
            }

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

            return productToUpdate;  
        }




    }
}

