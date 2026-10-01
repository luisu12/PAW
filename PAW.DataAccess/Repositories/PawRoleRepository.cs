using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IPawRoleRepository : IRepositoryBase<PawRole>
{
    new Task<bool> UpsertAsync(PawRole entity, bool isUpdating);
    new Task<bool> CreateAsync(PawRole entity);
    new Task<bool> DeleteAsync(PawRole entity);
    new Task<IEnumerable<PawRole>> ReadAsync();
    new Task<PawRole> FindAsync(int id);
    new Task<bool> UpdateAsync(PawRole entity);
    new Task<bool> UpdateManyAsync(IEnumerable<PawRole> entities);
    new Task<bool> ExistsAsync(PawRole entity);
}

public class PawRoleRepository : RepositoryBase<PawRole>, IPawRoleRepository
{
}
