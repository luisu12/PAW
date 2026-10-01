using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PawRoleController(ILogger<PawRoleController> logger, IPawRoleRepository pawRoleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetPawRoles")]
        public async Task<IEnumerable<PawRoleDTO>> GetAll()
        {
            var roles = await pawRoleRepository.ReadAsync() ?? [];
            return roles.Select(PawRoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetPawRoleById")]
        public async Task<ActionResult<PawRoleDTO>> GetById(int id)
        {
            var role = await pawRoleRepository.FindAsync(id);
            return PawRoleDTO.ConvertFrom(role);
        }

        /*[HttpPost("filter", Name = "FilterPawRoles")]
        public async Task<IEnumerable<PawRole>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<PawRole>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessPawRole.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<PawRole> Roles)
        {
            foreach (var p in Roles)
            {
                if (p.RoleId > 0)
                    await pawRoleRepository.CreateAsync(p);
                else
                    await pawRoleRepository.UpdateAsync(p);
            }

            /*Roles.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await pawRoleRepository.CreateAsync(x);
                else
                    await pawRoleRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(PawRole pawRole)
        {
            return await pawRoleRepository.DeleteAsync(pawRole);
        }
    }
}