using business_logic_layer;
using business_logic_layer.ViewModel;
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
           
            if (cart == null)
            {
                return BadRequest();
            }

            //string sessionId = cart.sessionId;
            CartModel result = await _cartBLL.AddCart(cart, cart.sessionId);
            return result;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartModel>>> GetCart(string sessionId)
        {
           
            var cart = await _cartBLL.GetCart(sessionId);
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart(string sessionId)
        {
            await _cartBLL.ClearCart(sessionId);
            return Ok(new { message = "Cart Cleared" });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId, string sessionId)
        {
            await _cartBLL.RemoveCart(productId, sessionId);
            return Ok(new { message = "Product removed from cart" });
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<CartModel>> UpdateProductQuantity(Guid productId, string sessionId)
        {
            Console.WriteLine($"sessionId sessionId sessionId: {sessionId}");
            var updatedProduct = await _cartBLL.UpdateCartQuantity(productId, sessionId);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct); // or simply return updatedProduct;
        }



    }
}
