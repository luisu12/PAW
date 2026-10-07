using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserRoleService
{
    Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync();
    Task<UserRoleDTO?> GetUserRoleByIdAsync(decimal id);
    Task<bool> CreateUserRoleAsync(UserRoleDTO userRole);
    Task<bool> UpdateUserRoleAsync(decimal id, UserRoleDTO userRole);
    Task<bool> DeleteUserRoleAsync(decimal id);
}

public class UserRoleService : ServiceBase, IUserRoleService
{
    private const string _path = "UserRole";
    private readonly IRestProvider _restProvider;

    public UserRoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var userRoles = await JsonProvider.DeserializeAsync<IEnumerable<UserRoleDTO>>(response);
        return userRoles;
    }

    public async Task<UserRoleDTO?> GetUserRoleByIdAsync(decimal id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var userRole = JsonProvider.DeserializeSimple<UserRoleDTO>(response);
        return userRole;
    }

    public async Task<bool> CreateUserRoleAsync(UserRoleDTO userRole)
    {
        // API Save endpoint expects an array of UserRole objects
        var content = JsonProvider.Serialize(new[] { userRole });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateUserRoleAsync(decimal id, UserRoleDTO userRole)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { userRole });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteUserRoleAsync(decimal id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
