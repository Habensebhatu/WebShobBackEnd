using System.Threading.Tasks;
using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductBLL _productBLL;

        public ProductController()
        {
            _productBLL = new ProductBLL();
        }

        [HttpPost]
        public async Task<ActionResult<productModel>> AddProduct([FromForm] List<IFormFile> files, [FromForm] string product)
        {
            productModel productData = JsonConvert.DeserializeObject<productModel>(product);

            if (productData == null)
            {
                return BadRequest();
            }
            Console.WriteLine($"Number of received files: {files.Count}");
            productData.NewImages = files;
            productModel result = await _productBLL.AddProduct(productData);

            return result;
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<productModelS>>> GetProductsByName(string category, [FromQuery] int pageNumber , [FromQuery] int pageSize)
        {
            var product = await _productBLL.GetProductsByName(category, pageNumber, pageSize);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpGet("category/{category}/price")]
        public async Task<ActionResult<List<productModelS>>> GetProductsByNameAndPrice(
     string category,
     [FromQuery] decimal minPrice,
     [FromQuery] decimal? maxPrice,
     [FromQuery] int pageNumber,
     [FromQuery] int pageSize)
        {
            var product = await _productBLL.GetProductsByNameAndPrice(category, minPrice, maxPrice, pageNumber, pageSize);
            if (product == null || !product.Any())
            {
                return NotFound();
            }
            return product;
        }




        [HttpGet]
        public async Task<ActionResult<List<productModelS>>> GetProducts()
        {
            var products = await _productBLL.GetProducts();
            return products;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<productModelS>> GetProductById(Guid id)
        {
            return await _productBLL.GetProductById(id);
        }

        [HttpGet("{product}")]
        public async Task<StripeImage> GetProductsByProductName(string product)
        {
            return await _productBLL.GetProductsByProductName(product);
        }


        [HttpGet("filterPrice/{min}/{max}")]
        public async Task<ActionResult<List<StripeImage>>> fillterPrice([FromRoute] Decimal min, [FromRoute] Decimal max)
        {
            var product = await _productBLL.fillterPrice(min, max);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpGet("search/{productName}")]
        public async Task<ActionResult<List<productModelS>>> SearchProductsByProductName(string productName)
        {
            var product = await _productBLL.SearchProductsByProductName(productName);

            Console.WriteLine($"productName productName productName: {product[0].Description}");
            return product;
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            await _productBLL.RemoveProduct(id);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<productModel>> UpdateProduct([FromForm] List<IFormFile> newImages, [FromForm] List<int> newImageIndices, [FromForm] string product, [FromForm] string existingImages)
        {
            productModel productData = JsonConvert.DeserializeObject<productModel>(product);
            List<ExistingImageUrlModel> existingImageUrls = JsonConvert.DeserializeObject<List<ExistingImageUrlModel>>(existingImages);

            if (productData == null || productData.productId == Guid.Empty)
            {
                return BadRequest();
            }
            productData.NewImages = newImages;
            productData.NewImageIndices = newImageIndices;
            productData.ExistingImageUrls = existingImageUrls;

            productModel result = await _productBLL.UpdateProduct(productData);

            return result;
        }


        [HttpGet("popular")]
        public async Task<ActionResult<List<productModelS>>> GetPopularProducts()
        {
            var popularProducts = await _productBLL.GetPopularProducts();
            return Ok(popularProducts);
        }



    }
}
