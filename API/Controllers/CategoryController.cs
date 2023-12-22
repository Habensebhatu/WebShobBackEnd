using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryBLL _CategorBLL;

        public CategoryController()
        {
            _CategorBLL = new CategoryBLL();
        }

        [HttpPost("AddCategory")]
        public async Task<ActionResult<CategoryModel>> AddCategoryItem([FromBody] CategoryModel category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            CategoryModel result = await _CategorBLL.AddCategory(category);
            return result;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategories()
        {
            return await _CategorBLL.GetCategories();
        }

        [HttpGet("GetCategoryById/{id:guid}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryById(Guid id)
        {
            return await _CategorBLL.GetCategoryById(id);
        }

        [HttpGet("GetCategoryByName/{category}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryByName(string category)
        {
            return await _CategorBLL.GetCategoryByCategory(category);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<CategoryModel>> DeleteCategory(Guid id)
        {
            return await _CategorBLL.RemoveCategory(id);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult<CategoryModel>> UpdateCategoryItem(Guid id, [FromBody] CategoryModel category)
        {
            var updatedCategory = await _CategorBLL.UpdateCategory(id, category);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            return updatedCategory;
        }
    }
}
