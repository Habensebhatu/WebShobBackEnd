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

            CartModel result = await _cartBLL.AddCart(cart);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartModel>>> GetCart()
        {
            var cart = await _cartBLL.GetCart();
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _cartBLL.ClearCart();
            return Ok(new { message = "Cart Cleared" });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            await _cartBLL.RemoveCart(productId);
            return Ok(new { message = "Product removed from cart" });
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<CartModel>> UpdateProductQuantity(Guid productId)
        {
            var updatedProduct = await _cartBLL.UpdateCartQuantity(productId);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct); // or simply return updatedProduct;
        }



    }
}
