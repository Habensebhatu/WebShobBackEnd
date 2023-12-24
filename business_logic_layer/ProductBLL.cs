using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;


namespace business_logic_layer
{
    public class ProductBLL
    {
        private readonly CategoryDNL _CategoryDAL;
        private readonly ProductDAL _ProductDAL;
        private readonly string azureConnectionString = "DefaultEndpointsProtocol=https;AccountName=imagestorewebshop;AccountKey=zF/V2og9TL6djw1t5q5Ej85iIv6gTRXFYZYYGNM2mQCL9GqiIJPkxcJ1oaLiDvdXwjukLUGjpArJ+ASteuO8tg==;EndpointSuffix=core.windows.net";

        public ProductBLL()
        {
            _ProductDAL = new ProductDAL();
            _CategoryDAL = new CategoryDNL();
        }

        public async Task<productModel> AddProduct(productModel product)
        {
            Console.WriteLine($"Incoming product images count: {product.NewImages.Count}");

            List<string> imageUrls = new List<string>();
            foreach (var image in product.NewImages)
            {
                imageUrls.Add(await UploadImageToAzure(image));
            }

            Product productFormat = new Product()
            {
                productId = product.productId,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                IsPopular = product.IsPopular,

            };
            Console.WriteLine($"business_logic_layer imageUrls count: {imageUrls.Count}");


            await _ProductDAL.AddProduct(productFormat, imageUrls);

            return product;
        }


        private async Task<string> UploadImageToAzure(IFormFile image)
        {
            var blobServiceClient = new BlobServiceClient(azureConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("blolcontainerws");
            string url = string.Empty;

            if (image != null && image.Length > 0)
            {
                var blobClient = blobContainerClient.GetBlobClient(image.FileName);

                using (var stream = image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
                }

                url = blobClient.Uri.AbsoluteUri;
            }

            return url;
        }

        public async Task<List<productModelS>> GetProducts()
        {
            var products = await _ProductDAL.GetProducts();
            return products.Select(p => new productModelS
            {
                productId = p.productId,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsPopular = p.IsPopular,
                ImageUrls = p.ProductImages
                    .OrderBy(pi => pi.Index)  // <-- Order the ProductImages by their Index here
                    .Select(pi => new ImageUpdateModel
                    {
                        Index = pi.Index,
                        File = pi.ImageUrl // Assuming ImageUrl is the file path or URL you want
                    }).ToList()
            }).ToList();
        }

        public async Task<List<productModelS>> GetProductsByName(string category, int pageNumber, int pageSize)
        {
            var products = await _ProductDAL.GetProductsByName(category, pageNumber, pageSize);

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

        public async Task<List<productModelS>> GetProductsPageNumber(int pageNumber, int pageSize)
        {
            var products = await _ProductDAL.GetProductsPageNumber(pageNumber, pageSize);

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

        public async Task<List<productModelS>> GetProductsByNameAndPrice(string category, decimal minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var products = await _ProductDAL.GetProductsByNameAndPrice(category, minPrice, maxPrice, pageNumber, pageSize);

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

        public async Task<productModelS> GetProductById(Guid id)
        {
            var products = await _ProductDAL.GetProductById(id);
            if (products == null)
            {
                return null;
            }
            return new productModelS
            {
                productId = products.productId,
                Title = products.Title,
                Price = products.Price,
                Description = products.Description,
                ImageUrls = products.ProductImages
                    .OrderBy(pi => pi.Index)  
                    .Select(pi => new ImageUpdateModel
                    {
                        Index = pi.Index,
                        File = pi.ImageUrl 
                    }).ToList(),
                CategoryId = products.CategoryId,
                CategoryName = products.Category.Name
            };
        }

        public async Task<List<productModelS>> SearchProductsByProductName(string product)
        {
            var products = await _ProductDAL.SearchProductsByProductName(product);
            return products.Select(p => new productModelS
            {
                productId = p.productId,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsPopular = p.IsPopular,
                ImageUrls = p.ProductImages
                   .OrderBy(pi => pi.Index)
                   .Select(pi => new ImageUpdateModel
                   {
                       Index = pi.Index,
                       File = pi.ImageUrl
                   }).ToList()
            }).ToList();

        }

      

        public async Task<StripeImage> GetProductsByProductName(string product)
        {
            Console.WriteLine($"productrrrrrrr: {product}");
            var products = await _ProductDAL.GetProductsByProductName(product);

            return new StripeImage
            {
                Title = products.Title,
                Price = products.Price,
                Description = products.Description,
                CategoryId = products.CategoryId,
                productId = products.productId,
                ImageUrls = products.ProductImages?.FirstOrDefault()?.ImageUrl

            };

        }

        public async Task<List<StripeImage>> fillterPrice(decimal min, decimal max)
        {
            var products = await _ProductDAL.fillterPrice(min, max);

            return products.Select(p => new StripeImage
            {
                productId = p.productId,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                ImageUrls = p.ProductImages?.FirstOrDefault()?.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
        }



        public async Task RemoveProduct(Guid id)
        {
            await _ProductDAL.RemoveProduct(id);
        }



        public async Task<productModel> UpdateProduct(productModel product)
        {
            var categoryBYName = await _CategoryDAL.GetCategoryByName(product.CategoryName);
            List<ExistingImageUrlModel> allImageUrls = new List<ExistingImageUrlModel>(product.ExistingImageUrls);

            for (int i = 0; i < product.NewImages.Count; i++)
            {
                var image = product.NewImages[i];
                var index = product.NewImageIndices[i]; // Fetch the index

                var imageUrl = await UploadImageToAzure(image);
                allImageUrls.Add(new ExistingImageUrlModel
                {
                    file = imageUrl,
                    index = index
                });
            }

            Product productFormat = new Product()
            {
                productId = product.productId,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                CategoryId = categoryBYName.CategoryId,
                IsPopular = product.IsPopular
            };
            var formattedImages = allImageUrls.Select(i => new ExistingImageMode()
            {
                file = i.file,
                index = i.index
            }).ToList();

            await _ProductDAL.UpdateProduct(productFormat, formattedImages);

            return product;
        }

        public async Task<List<productModelS>> GetPopularProducts()
        {
            var products = await _ProductDAL.GetPopularProducts();
            return products.Select(p => new productModelS
            {
                productId = p.productId,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsPopular = p.IsPopular,
                ImageUrls = p.ProductImages
                    .OrderBy(pi => pi.Index)
                    .Select(pi => new ImageUpdateModel
                    {
                        Index = pi.Index,
                        File = pi.ImageUrl
                    }).ToList()
            }).ToList();

        }

        public async Task<List<productModelS>> GetProductsByCategory(string category)
        {
            var products = await _ProductDAL.GetProductsByCategory(category);
            return products.Select(p => new productModelS
            {
                productId = p.productId,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsPopular = p.IsPopular,
                ImageUrls = p.ProductImages
                    .OrderBy(pi => pi.Index)
                    .Select(pi => new ImageUpdateModel
                    {
                        Index = pi.Index,
                        File = pi.ImageUrl
                    }).ToList()
            }).ToList();

        }

    }
}

