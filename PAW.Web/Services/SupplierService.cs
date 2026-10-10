using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
    Task<SupplierDTO?> GetSupplierByIdAsync(int id);
    Task<bool> CreateSupplierAsync(SupplierDTO supplier);
    Task<bool> UpdateSupplierAsync(int id, SupplierDTO supplier);
    Task<bool> DeleteSupplierAsync(int id);
}

public class SupplierService : ServiceBase, ISupplierService
{
    private const string _path = "Supplier";
    private readonly IRestProvider _restProvider;

    public SupplierService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<SupplierDTO>> GetSuppliersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        // No-op change: touch file to trigger patch group
        var suppliers = await JsonProvider.DeserializeAsync<IEnumerable<SupplierDTO>>(response);
        return suppliers;
    }

    public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var supplier = JsonProvider.DeserializeSimple<SupplierDTO>(response);
        return supplier;
    }

    public async Task<bool> CreateSupplierAsync(SupplierDTO supplier)
    {
        // API expects an array of Supplier objects
        var content = JsonProvider.Serialize(new[] { supplier });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateSupplierAsync(int id, SupplierDTO supplier)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { supplier });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
