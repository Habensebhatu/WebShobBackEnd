using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Http;
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
        [HttpPost]
        public async Task<ActionResult<CategoryModel>> addCategory([FromBody] CategoryModel category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            CategoryModel result = await _CategorBLL.AddCategory(category);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> getCategories()
        {
            return await _CategorBLL.GetCategories();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryModel>> getCategoryById(Guid id)
        {
            return await _CategorBLL.GetCategoryById(id);
        }

        [HttpGet("{category}")]
        public async Task<ActionResult<CategoryModel>> getCategoryByCategory(string category)
        {
            return await _CategorBLL.GetCategoryByCategory(category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryModel>> removeCategory(Guid id)
        {
            return await _CategorBLL.RemoveCategory(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryModel>> updateCategory(Guid id, [FromBody] CategoryModel category)
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
