using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetCategories")]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await categoryRepository.ReadAsync() ?? [];
            return categories.Select(CategoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            return CategoryDTO.ConvertFrom(category);
        }

        /*[HttpPost("filter", Name = "FilterCategories")]
        public async Task<IEnumerable<Category>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<Category>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessCategory.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Category> Categories)
        {
            foreach (var p in Categories)
            {
                // Update existing when CategoryId > 0, otherwise create new
                if (p.CategoryId > 0)
                    await categoryRepository.UpdateAsync(p);
                else
                    await categoryRepository.CreateAsync(p);
            }

            /*Categories.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await categoryRepository.CreateAsync(x);
                else
                    await categoryRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            if (category == null) return false;
            return await categoryRepository.DeleteAsync(category);
        }
    }
}
