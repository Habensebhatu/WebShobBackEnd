using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class CategoryDNL
    {

        private readonly MyDbContext _context;
        public CategoryDNL()
        {
            _context = new MyDbContext();
        }

        public async Task<Category> AddCategory(Category customer)
        {
            _context.Category.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
        }

        public async Task<Category> GetCategoryByName(string categoryId)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.Name == categoryId);
        }

        public async Task<Category> RemoveCategory(Guid categoryId)
        {
            var category = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
            if (category != null)
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            return category;
        }

        public async Task UpdateCategory()
        {
            await _context.SaveChangesAsync();
        }

    }
}

