using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class ProductDAL
    {
        private readonly MyDbContext _context;
        public ProductDAL()
        {
            _context = new MyDbContext();
        }



        public async Task<Product> AddProduct(Product product, List<string> imageUrls)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
         
            for (int i = 0; i < imageUrls.Count; i++)
            {
                var productImage = new ProductImageEnityModel
                {
                    ImageUrl = imageUrls[i],
                    ProductId = product.productId,
                    Index = i  
                };
                _context.ProductImage.Add(productImage);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex.Message", ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("ex.InnerException.Message", ex.InnerException.Message);
                }
            }

            return product;
        }



        public async Task<List<Product>> GetProducts()
        {
            return await _context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderBy(p => p.ProductImages.Min(img => img.Index))
                .ToListAsync();
        }



        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Product.Include(p => p.Category).Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.productId == id);
        }



        public async Task<List<Product>> GetProductsByName(string category, int pageNumber, int pageSize)
        {
            return await _context.Product.Include(p => p.Category)
                                         .Include(p => p.ProductImages)
                                         .Where(p => p.Category.Name == category)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByNameAndPrice(string category, decimal minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var query = _context.Product.Include(p => p.Category)
                                        .Include(p => p.ProductImages)
                                        .Where(p => p.Category.Name == category && p.Price >= minPrice);

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }




        public async Task<List<Product>> SearchProductsByProductName(string product)
        {
            return await _context.Product.Include(p => p.Category).Include(p => p.ProductImages).Where(p => p.Title.StartsWith(product)).ToListAsync();

        }

        public async Task<Product> GetProductsByProductName(string product)
        {
            return await _context.Product.Include(p => p.Category).Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Title.Contains(product));
        }

        public async Task<List<Product>> fillterPrice(decimal min, decimal max)
        {
            return await _context.Product.Include(p => p.Category).Where(p => p.Price >= min && p.Price <= max).ToListAsync();
        }

        public async Task RemoveProduct(Guid id)
        {
            var product = await GetProductById(id);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Product> UpdateProduct(Product product, List<ExistingImageMode> imageModels)
        {
            var existingProduct = _context.Product.Find(product.productId);
            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                var existingImages = _context.ProductImage.Where(pi => pi.ProductId == product.productId).ToList();

     
                foreach (var existingImage in existingImages)
                {
                    if (!imageModels.Any(imgModel => imgModel.file == existingImage.ImageUrl))
                    {
                        _context.ProductImage.Remove(existingImage);
                    }
                }

                foreach (var imageModel in imageModels)
                {
                    if (!existingImages.Any(ei => ei.ImageUrl == imageModel.file))
                    {
                        var productImage = new ProductImageEnityModel
                        {
                            ImageUrl = imageModel.file,
                            ProductId = product.productId,
                            Index = imageModel.index
                        };
                        _context.ProductImage.Add(productImage);
                    }
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Product not found!");
            }

            return product;
        }

        public async Task<List<Product>> GetPopularProducts()
        {
            return await _context.Product.Include(p => p.Category).Include(p => p.ProductImages).Where(p => p.IsPopular).ToListAsync();
        }



    }
}

