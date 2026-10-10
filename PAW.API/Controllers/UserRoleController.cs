using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
        public async Task<bool> Save([FromBody] IEnumerable<PAW.Models.DTO.UserRoleDTO> UserRoles)
        {
            foreach (var dto in UserRoles)
            {
                var p = PAW.Models.DTO.UserRoleDTO.ConvertTo(dto);
                // Create when id is 0 (new), update when id > 0 (existing)
                if (p.Id > 0)
                    await userRoleRepository.UpdateAsync(p);
                else
                    await userRoleRepository.CreateAsync(p);
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

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var userRole = await userRoleRepository.FindAsync(id);
            if (userRole == null) return false;
            return await userRoleRepository.DeleteAsync(userRole);
        }
    }
}