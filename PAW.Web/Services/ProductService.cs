using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
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

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var products = await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);
        return products;
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
