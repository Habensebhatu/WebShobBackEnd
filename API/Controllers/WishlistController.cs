using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly WishlistBLL _wishlistBLL;

        public WishlistController()
        {
            _wishlistBLL = new WishlistBLL();
        }

        [Authorize] 
        [HttpPost("AddToWishList")]
        public async Task<ActionResult<WishlistModel>> AddToWishList(AddToWishlistRequest request)
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var productId = request.ProductId;
            Console.WriteLine($"userId is gevonden : {userId}");
            Console.WriteLine($"productId  : {productId}");
            
            if (string.IsNullOrWhiteSpace(userId) || productId == null)
            {
                return BadRequest();
            }
            if (!Guid.TryParse(productId, out var parsedProductId))
            {
                return BadRequest("Invalid productId format");
            }
            return await _wishlistBLL.AddProductToWishlist(parsedProductId, userId);
        }

        [Authorize]
        [HttpGet("GetWishlistProducts")]
        public async Task<ActionResult<List<productModelS>>> GetWishlistProducts()
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("Invalid userId");
            }

            List<productModelS> products = await _wishlistBLL.GetWishlistProducts(userId);

            return Ok(products); 
        }

        [Authorize]
        [HttpDelete("DeleteFromWishlist/{productId}")]
        public async Task<ActionResult<bool>> DeleteWishListProductByID(Guid productId)
        {
            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId) || productId == Guid.Empty)
            {
                return BadRequest("Invalid input");
            }

            var result = await _wishlistBLL.DeleteProductFromWishlist(productId, userId);
            if (result)
            {
                return result;
            }
            else
            {
                return NotFound("Product not found in wishlist.");
            }
        }


    }
}
