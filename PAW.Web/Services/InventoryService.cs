using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetInventoriesAsync();
    Task<InventoryDTO?> GetInventoryByIdAsync(int id);
    Task<bool> CreateInventoryAsync(InventoryDTO inventory);
    Task<bool> UpdateInventoryAsync(int id, InventoryDTO inventory);
    Task<bool> DeleteInventoryAsync(int id);
}

public class InventoryService : ServiceBase, IInventoryService
{
    private const string _path = "Inventory";
    private readonly IRestProvider _restProvider;

    public InventoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<InventoryDTO>> GetInventoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var inventories = await JsonProvider.DeserializeAsync<IEnumerable<InventoryDTO>>(response);
        return inventories;
    }

    public async Task<InventoryDTO?> GetInventoryByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var inventory = JsonProvider.DeserializeSimple<InventoryDTO>(response);
        return inventory;
    }

    public async Task<bool> CreateInventoryAsync(InventoryDTO inventory)
    {
        // API Save endpoint expects an array of Inventory objects
        var content = JsonProvider.Serialize(new[] { inventory });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateInventoryAsync(int id, InventoryDTO inventory)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { inventory });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteInventoryAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
