using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawRoleService
{
    Task<IEnumerable<PawRoleDTO>> GetPawRolesAsync();
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
}
