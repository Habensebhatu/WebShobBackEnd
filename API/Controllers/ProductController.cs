using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<productModel>> addProduct([FromForm] List<IFormFile> files, [FromForm] string product)
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


        [HttpGet]
        public async Task<ActionResult<List<productModelS>>> GetProducts()
        {
            var products = await _productBLL.GetProducts();
            return products;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<productModelS>> getCategoryById(Guid id)
        {
            return await _productBLL.GetProductById(id);
        }

        [HttpGet("{product}")]
        public async Task<StripeImage> getproductbyprodutName(string product)
        {
            return await _productBLL.GetProductsByProductName(product);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<productModelS>>> GetProductsByName(string category)
        {
            var product = await _productBLL.GetProductsByName(category);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpGet("filterPrice/{min}/{max}")]
        public async Task<ActionResult<List<StripeImage>>>fillterPrice([FromRoute] Decimal min, [FromRoute] Decimal max)
        {
            var product = await _productBLL.fillterPrice(min, max);
            if (product == null)
            {
                return NotFound();
            }
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

            Console.WriteLine($"Number of received files for update: {newImages.Count}");
            Console.WriteLine($"newImageIndices: {newImageIndices}");
            Console.WriteLine($"existingImageUrls: {existingImageUrls.Count}");
            productData.NewImages= newImages;
            productData.NewImageIndices = newImageIndices;
            productData.ExistingImageUrls = existingImageUrls;

            productModel result = await _productBLL.UpdateProduct(productData);

            return result;
        }


    }
}
