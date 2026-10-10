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
        public async Task<bool> Save([FromBody] IEnumerable<PAW.Models.DTO.UserActionDTO> UserActions)
        {
            try
            {
                var payload = System.Text.Json.JsonSerializer.Serialize(UserActions);
                logger.LogDebug($"UserAction Save payload: {payload}");
            }
            catch { }
            foreach (var dto in UserActions)
            {
                try
                {
                    logger.LogDebug($"UserAction DTO: Name='{dto.Name}', Description='{dto.Description}'");
                }
                catch { }
                var p = PAW.Models.DTO.UserActionDTO.ConvertTo(dto);
                // Normalize nullable Id to ensure primary key is set when present
                if (p.Id == null)
                {
                    p.Id = 0;
                }

                if (p.Id > 0)
                    await userActionRepository.UpdateAsync(p);
                else
                    await userActionRepository.CreateAsync(p);
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

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var userAction = await userActionRepository.FindAsync(id);
            if (userAction == null) return false;
            return await userActionRepository.DeleteAsync(userAction);
        }
    }
}