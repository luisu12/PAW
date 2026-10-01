using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserRoleController(ILogger<UserRoleController> logger, IUserRoleRepository userRoleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserRoles")]
        public async Task<IEnumerable<UserRoleDTO>> GetAll()
        {
            var roles = await userRoleRepository.ReadAsync() ?? [];
            return roles.Select(UserRoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserRoleById")]
        public async Task<ActionResult<UserRoleDTO>> GetById(int id)
        {
            var role = await userRoleRepository.FindAsync(id);
            return UserRoleDTO.ConvertFrom(role);
        }

        /*[HttpPost("filter", Name = "FilterUserRoles")]
        public async Task<IEnumerable<UserRole>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<UserRole>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessUserRole.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<UserRole> UserRoles)
        {
            foreach (var p in UserRoles)
            {
                if (p.Id > 0)
                    await userRoleRepository.CreateAsync(p);
                else
                    await userRoleRepository.UpdateAsync(p);
            }

            /*UserRoles.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await userRoleRepository.CreateAsync(x);
                else
                    await userRoleRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(UserRole userRole)
        {
            return await userRoleRepository.DeleteAsync(userRole);
        }
    }
}