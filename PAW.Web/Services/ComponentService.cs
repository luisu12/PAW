using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<IEnumerable<ComponentDTO>> GetComponentsAsync();
    Task<ComponentDTO?> GetComponentByIdAsync(decimal id);
    Task<bool> CreateComponentAsync(ComponentDTO component);
    Task<bool> UpdateComponentAsync(decimal id, ComponentDTO component);
    Task<bool> DeleteComponentAsync(decimal id);
}

public class ComponentService : ServiceBase, IComponentService
{
    private const string _path = "Component";
    private readonly IRestProvider _restProvider;

    public ComponentService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ComponentDTO>> GetComponentsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var components = await JsonProvider.DeserializeAsync<IEnumerable<ComponentDTO>>(response);
        return components;
    }

    public async Task<ComponentDTO?> GetComponentByIdAsync(decimal id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var component = JsonProvider.DeserializeSimple<ComponentDTO>(response);
        return component;
    }

    public async Task<bool> CreateComponentAsync(ComponentDTO component)
    {
        // API Save endpoint expects an array of Component objects
        var content = JsonProvider.Serialize(new[] { component });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateComponentAsync(decimal id, ComponentDTO component)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { component });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteComponentAsync(decimal id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
