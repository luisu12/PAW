using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger, IUserRepository userRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await userRepository.ReadAsync() ?? [];
            return users.Select(UserDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await userRepository.FindAsync(id);
            return UserDTO.ConvertFrom(user);
        }

        /*[HttpPost("filter", Name = "FilterUsers")]
        public async Task<IEnumerable<User>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<User>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessUser.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<User> Users)
        {
            foreach (var p in Users)
            {
                if (p.UserId > 0)
                    await userRepository.CreateAsync(p);
                else
                    await userRepository.UpdateAsync(p);
            }

            /*Users.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await userRepository.CreateAsync(x);
                else
                    await userRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(User user)
        {
            return await userRepository.DeleteAsync(user);
        }
    }
}