using PAW.Models;
using PAW.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PAW.DataAccess.Repositories;

public interface IUserRoleRepository : IRepositoryBase<UserRole>
{
    Task<bool> UpsertAsync(UserRole entity, bool isUpdating);
    Task<bool> CreateAsync(UserRole entity);
    Task<bool> DeleteAsync(UserRole entity);
    Task<IEnumerable<UserRole>> ReadAsync();
    Task<UserRole> FindAsync(decimal id); // Nota: Mapeado como decimal según script SQL
    Task<bool> UpdateAsync(UserRole entity);
    Task<bool> UpdateManyAsync(IEnumerable<UserRole> entities);
    Task<bool> ExistsAsync(UserRole entity);
}

public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
{
    public async Task<IEnumerable<UserRole>> ReadAsync()
    {
        // Use manual ADO read to avoid EF projection SqlNullValueException when DB contains unexpected NULLs
        var results = new List<UserRole>();
        var conn = DbContext.Database.GetDbConnection();
        try
        {
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, RoldID, UserID FROM UserRoles";
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var ur = new UserRole();
                // Id
                if (await reader.IsDBNullAsync(0)) ur.Id = null; else ur.Id = reader.GetDecimal(0);
                // RoldId
                if (await reader.IsDBNullAsync(1)) ur.RoldId = null; else ur.RoldId = reader.GetDecimal(1);
                // UserId
                if (await reader.IsDBNullAsync(2)) ur.UserId = null; else ur.UserId = reader.GetDecimal(2);
                results.Add(ur);
            }

            return results;
        }
        catch
        {
            // On any error, fallback to base behavior
            return await base.ReadAsync();
        }
        finally
        {
            try { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); } catch { }
        }
    }

}
