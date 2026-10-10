using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<(IEnumerable<CategoryDTO> Items, int TotalItems)> GetCategoriesAsync(int page = 1, int pageSize = 25);
    Task<CategoryDTO?> GetCategoryByIdAsync(int id);
    Task<bool> CreateCategoryAsync(CategoryDTO category);
    Task<bool> UpdateCategoryAsync(int id, CategoryDTO category);
    Task<bool> DeleteCategoryAsync(int id);
}

public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<(IEnumerable<CategoryDTO> Items, int TotalItems)> GetCategoriesAsync(int page = 1, int pageSize = 25)
    {
        var url = SetPathUrl(_path) + $"?page={page}&pageSize={pageSize}";
        var response = await _restProvider.GetAsync(url, id: null);
        try
        {
            var paged = await JsonProvider.DeserializeAsync<PAW.Models.PagedResultDTO<CategoryDTO>>(response);
            if (paged != null)
            {
                return (paged.Items ?? Enumerable.Empty<CategoryDTO>(), paged.TotalItems);
            }
        }
        catch (System.Text.Json.JsonException)
        {
            // fallback below
        }

        // If API still returns a bare array, handle that shape and page client-side
        var items = await JsonProvider.DeserializeAsync<IEnumerable<CategoryDTO>>(response);
        var list = (items ?? Enumerable.Empty<CategoryDTO>()).ToList();
        var totalCount = list.Count;
        var pagedItems = list.Skip((page - 1) * pageSize).Take(pageSize);
        return (pagedItems, totalCount);
    }

    public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var category = JsonProvider.DeserializeSimple<CategoryDTO>(response);
        return category;
    }

    public async Task<bool> CreateCategoryAsync(CategoryDTO category)
    {
        // API Save endpoint expects an array of Category objects in the body
        var content = JsonProvider.Serialize(new[] { category });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        // assume success if no exception; optionally deserialize response
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateCategoryAsync(int id, CategoryDTO category)
    {
        // API uses the same Save POST endpoint for create/update and expects an array
        var content = JsonProvider.Serialize(new[] { category });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }

}
