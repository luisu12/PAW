using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawRoleService
{
    Task<IEnumerable<PawRoleDTO>> GetPawRolesAsync();
    Task<PawRoleDTO?> GetPawRoleByIdAsync(int id);
    Task<bool> CreatePawRoleAsync(PawRoleDTO role);
    Task<bool> UpdatePawRoleAsync(int id, PawRoleDTO role);
    Task<bool> DeletePawRoleAsync(int id);
}

public class PawRoleService : ServiceBase, IPawRoleService
{
    private const string _path = "PawRole";
    private readonly IRestProvider _restProvider;

    public PawRoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<PawRoleDTO>> GetPawRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var roles = await JsonProvider.DeserializeAsync<IEnumerable<PawRoleDTO>>(response);
        return roles;
    }

    public async Task<PawRoleDTO?> GetPawRoleByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var role = JsonProvider.DeserializeSimple<PawRoleDTO>(response);
        return role;
    }

    public async Task<bool> CreatePawRoleAsync(PawRoleDTO role)
    {
        // API Save endpoint expects an array of PawRole objects
        var content = JsonProvider.Serialize(new[] { role });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdatePawRoleAsync(int id, PawRoleDTO role)
    {
        // API Save endpoint expects an array of PawRole objects (POST). Use POST for create/update.
        var content = JsonProvider.Serialize(new[] { role });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeletePawRoleAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
