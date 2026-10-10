using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ComponentController(ILogger<ComponentController> logger, IComponentRepository componentRepository) : ControllerBase
    {
        [HttpGet(Name = "GetComponents")]
        public async Task<IEnumerable<ComponentDTO>> GetAll()
        {
            var components = await componentRepository.ReadAsync() ?? [];
            return components.Select(ComponentDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(int id)
        {
            var component = await componentRepository.FindAsync(id);
            return ComponentDTO.ConvertFrom(component);
        }

        /*[HttpPost("filter", Name = "FilterComponents")]
        public async Task<IEnumerable<Component>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<Component>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessComponent.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Component> Components)
        {
            foreach (var p in Components)
            {
                // Update when existing, create when new
                if (p.Id > 0)
                    await componentRepository.UpdateAsync(p);
                else
                    await componentRepository.CreateAsync(p);
            }

            /*Components.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await componentRepository.CreateAsync(x);
                else
                    await componentRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var component = await componentRepository.FindAsync(id);
            if (component == null) return false;
            return await componentRepository.DeleteAsync(component);
        }
    }
}