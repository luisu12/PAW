using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserActionController(ILogger<UserActionController> logger, IUserActionRepository userActionRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserActions")]
        public async Task<IEnumerable<UserActionDTO>> GetAll()
        {
            var actions = await userActionRepository.ReadAsync() ?? [];
            return actions.Select(UserActionDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserActionById")]
        public async Task<ActionResult<UserActionDTO>> GetById(int id)
        {
            var action = await userActionRepository.FindAsync(id);
            return UserActionDTO.ConvertFrom(action);
        }

        /*[HttpPost("filter", Name = "FilterUserActions")]
        public async Task<IEnumerable<UserAction>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<UserAction>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessUserAction.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<UserAction> UserActions)
        {
            foreach (var p in UserActions)
            {
                if (p.Id > 0)
                    await userActionRepository.CreateAsync(p);
                else
                    await userActionRepository.UpdateAsync(p);
            }

            /*UserActions.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await userActionRepository.CreateAsync(x);
                else
                    await userActionRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(UserAction userAction)
        {
            return await userActionRepository.DeleteAsync(userAction);
        }
    }
}