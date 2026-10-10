using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTaskService
{
    Task<IEnumerable<PawTaskDTO>> GetPawTasksAsync();
    Task<PawTaskDTO?> GetPawTaskByIdAsync(int id);
    Task<bool> CreatePawTaskAsync(PawTaskDTO task);
    Task<bool> UpdatePawTaskAsync(int id, PawTaskDTO task);
    Task<bool> DeletePawTaskAsync(int id);
}

public class PawTaskService : ServiceBase, IPawTaskService
{
    private const string _path = "PawTask";
    private readonly IRestProvider _restProvider;

    public PawTaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<PawTaskDTO>> GetPawTasksAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var tasks = await JsonProvider.DeserializeAsync<IEnumerable<PawTaskDTO>>(response);
        return tasks;
    }

    public async Task<PawTaskDTO?> GetPawTaskByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var task = JsonProvider.DeserializeSimple<PawTaskDTO>(response);
        return task;
    }

    public async Task<bool> CreatePawTaskAsync(PawTaskDTO task)
    {
        // API Save endpoint expects an array of PawTask objects
        var content = JsonProvider.Serialize(new[] { task });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdatePawTaskAsync(int id, PawTaskDTO task)
    {
        // API Save endpoint expects an array of PawTask objects via POST
        var content = JsonProvider.Serialize(new[] { task });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeletePawTaskAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
