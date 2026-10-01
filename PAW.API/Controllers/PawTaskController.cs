using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PawTaskController(ILogger<PawTaskController> logger, IPawTaskRepository pawTaskRepository) : ControllerBase
    {
        [HttpGet(Name = "GetPawTasks")]
        public async Task<IEnumerable<PawTaskDTO>> GetAll()
        {
            var tasks = await pawTaskRepository.ReadAsync() ?? [];
            return tasks.Select(PawTaskDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetPawTaskById")]
        public async Task<ActionResult<PawTaskDTO>> GetById(int id)
        {
            var task = await pawTaskRepository.FindAsync(id);
            return PawTaskDTO.ConvertFrom(task);
        }

        /*[HttpPost("filter", Name = "FilterPawTasks")]
        public async Task<IEnumerable<PawTask>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<PawTask>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessPawTask.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<PawTask> Tasks)
        {
            foreach (var p in Tasks)
            {
                if (p.Id > 0)
                    await pawTaskRepository.CreateAsync(p);
                else
                    await pawTaskRepository.UpdateAsync(p);
            }

            /*Tasks.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await pawTaskRepository.CreateAsync(x);
                else
                    await pawTaskRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(PawTask task)
        {
            return await pawTaskRepository.DeleteAsync(task);
        }
    }
}