using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Linq;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<(IEnumerable<ProductDTO> Items, int TotalItems)> GetProductsAsync(int page = 1, int pageSize = 25);
    Task<ProductDTO?> GetProductByIdAsync(int id);
    Task<bool> CreateProductAsync(ProductDTO product);
    Task<bool> UpdateProductAsync(int id, ProductDTO product);
    Task<bool> DeleteProductAsync(int id);
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<(IEnumerable<ProductDTO> Items, int TotalItems)> GetProductsAsync(int page = 1, int pageSize = 25)
    {
        var url = SetPathUrl(_path) + $"?page={page}&pageSize={pageSize}";
        var response = await _restProvider.GetAsync(url, id: null);
        try
        {
            var paged = await JsonProvider.DeserializeAsync<PAW.Models.PagedResultDTO<ProductDTO>>(response);
            if (paged != null)
            {
                return (paged.Items ?? Enumerable.Empty<ProductDTO>(), paged.TotalItems);
            }
        }
        catch (System.Text.Json.JsonException)
        {
            // fallback below
        }

        // If API still returns a bare array, handle that shape and page client-side
        var items = await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);
        var list = (items ?? Enumerable.Empty<ProductDTO>()).ToList();
        var totalCount = list.Count;
        var pagedItems = list.Skip((page - 1) * pageSize).Take(pageSize);
        return (pagedItems, totalCount);
    }

    public async Task<ProductDTO?> GetProductByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var product = JsonProvider.DeserializeSimple<ProductDTO>(response);
        return product;
    }

    public async Task<bool> CreateProductAsync(ProductDTO product)
    {
        // API Save endpoint expects an array of Product objects
        var content = JsonProvider.Serialize(new[] { product });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateProductAsync(int id, ProductDTO product)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { product });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
