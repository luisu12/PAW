using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserActionService
{
    Task<IEnumerable<UserActionDTO>> GetUserActionsAsync();
    Task<UserActionDTO?> GetUserActionByIdAsync(decimal id);
    Task<bool> CreateUserActionAsync(UserActionDTO action);
    Task<bool> UpdateUserActionAsync(decimal id, UserActionDTO action);
    Task<bool> DeleteUserActionAsync(decimal id);
}

public class UserActionService : ServiceBase, IUserActionService
{
    private const string _path = "UserAction";
    private readonly IRestProvider _restProvider;

    public UserActionService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserActionDTO>> GetUserActionsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var actions = await JsonProvider.DeserializeAsync<IEnumerable<UserActionDTO>>(response);
        return actions;
    }

    public async Task<UserActionDTO?> GetUserActionByIdAsync(decimal id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var action = JsonProvider.DeserializeSimple<UserActionDTO>(response);
        return action;
    }

    public async Task<bool> CreateUserActionAsync(UserActionDTO action)
    {
        // API Save endpoint expects an array of UserAction objects
        var content = JsonProvider.Serialize(new[] { action });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateUserActionAsync(decimal id, UserActionDTO action)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { action });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteUserActionAsync(decimal id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
