using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;

namespace business_logic_layer
{
	public class CategoryBLL
	{

        private readonly CategoryDNL _CategoryDAL;
        public CategoryBLL()
        {
            _CategoryDAL = new CategoryDNL();
        }
        public async Task<CategoryModel> AddCategory(CategoryModel category)
        {
            Category categoryFormaat = new Category()
            {
                CategoryId = category.categoryId,
                Name = category.Name,
                quantityProduct = category.quantityProduct
               
            };
            await _CategoryDAL.AddCategory(categoryFormaat);
            return category;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
          List<Category> categories = await _CategoryDAL.GetCategories();
           List<CategoryModel> categoriesModel = categories.Select(c => new CategoryModel
           { categoryId = c.CategoryId,
             Name = c.Name, quantityProduct = c.quantityProduct}).ToList();

            return categoriesModel;
        }

        public async Task<CategoryModel> GetCategoryById(Guid id)
        {
            var category = await _CategoryDAL.GetCategoryById(id);
            return new CategoryModel { categoryId = category.CategoryId, Name = category.Name , quantityProduct = category.quantityProduct};
        }

        public async Task<CategoryModel> GetCategoryByCategory(string category)
        {
            var categoryBYName = await _CategoryDAL.GetCategoryByName(category);
            return new CategoryModel { categoryId = categoryBYName.CategoryId, Name = categoryBYName.Name, quantityProduct = categoryBYName.quantityProduct };
        }

        public async Task<CategoryModel> RemoveCategory(Guid id)
        {
            var category = await _CategoryDAL.RemoveCategory(id);
            return new CategoryModel { categoryId = category.CategoryId, Name = category.Name };
        }

        public async Task<CategoryModel> UpdateCategory(Guid categoryId, CategoryModel category)
        {
            var existingCategory =  await _CategoryDAL.GetCategoryById(categoryId);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;
            existingCategory.quantityProduct = category.quantityProduct;
           
            try
            {
                await _CategoryDAL.UpdateCategory();
            }
            catch (Exception ex)
            {
                return null;
            }
            return new CategoryModel
            {
                categoryId = existingCategory.CategoryId,
                Name = existingCategory.Name,
               quantityProduct = existingCategory.quantityProduct
            };
        }



    }


}

