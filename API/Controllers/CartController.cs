using System.Security.Claims;
using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CartController : ControllerBase
    {
        private readonly CartBLL _cartBLL;

        public CartController()
        {
            _cartBLL = new CartBLL();
        }

        [HttpPost]
        public async Task<ActionResult<CartModel>> AddToCart([FromBody] CartModel cart)
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = _cartBLL.GetDefaultUserId();  // Fetch the default user id
            }

            Console.WriteLine($"userId: {userId}");
            if (cart == null)
            {
                return BadRequest();
            }
            CartModel result = await _cartBLL.AddCart(cart, cart.sessionId, userId);
            return result;
        }



        [HttpGet]
        public async Task<ActionResult<List<CartModel>>> GetCart(string sessionId)
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = null;
            }
            Console.WriteLine($"userId: {userId}");

            var cart = await _cartBLL.GetCart(sessionId, userId);
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart(string sessionId)
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _cartBLL.ClearCart(sessionId, userId);
            return Ok(new { message = "Cart Cleared" });
        }


        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId, [FromQuery] string sessionId)
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // If the user is not logged in and there's no session id, return a bad request.
            if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(sessionId))
                return BadRequest(new { message = "Either User Id or Session Id is required." });

            await _cartBLL.RemoveCart(productId, userId, sessionId);
            return Ok(new { message = "Product removed from cart" });
        }


        [HttpPut("{productId}")]
        public async Task<ActionResult<CartModel>> UpdateProductQuantity(Guid productId, [FromQuery] string sessionId)
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var updatedProduct = await _cartBLL.UpdateCartQuantity(productId, userId, sessionId);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

    }
}
