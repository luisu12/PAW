using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int id);
    Task<bool> CreateUserAsync(UserDTO user);
    Task<bool> UpdateUserAsync(int id, UserDTO user);
    Task<bool> DeleteUserAsync(int id);
}

public class UserService : ServiceBase, IUserService
{
    private const string _path = "User";
    private readonly IRestProvider _restProvider;

    public UserService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var users = await JsonProvider.DeserializeAsync<IEnumerable<UserDTO>>(response);
        return users;
    }

    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var user = await JsonProvider.DeserializeAsync<UserDTO>(response);
        return user;
    }

    public async Task<bool> CreateUserAsync(UserDTO user)
    {
        // API Save endpoint expects an array of User objects
        var content = JsonProvider.Serialize(new[] { user });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateUserAsync(int id, UserDTO user)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { user });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
