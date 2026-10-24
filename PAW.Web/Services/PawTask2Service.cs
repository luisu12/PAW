using System.Net.Http.Json;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTask2Service
{
    Task<(IEnumerable<PawTask2DTO> Items, int TotalItems)> GetPawTask2ItemsAsync(int page = 1, int pageSize = 25, CancellationToken ct = default);
    Task<PawTask2DTO?> GetPawTask2ItemByIdAsync(int id, CancellationToken ct = default);
    Task<bool> CreatePawTask2Async(PawTask2DTO item, CancellationToken ct = default);
    Task<bool> UpdatePawTask2Async(int id, PawTask2DTO item, CancellationToken ct = default);
    Task<bool> DeletePawTask2Async(int id, CancellationToken ct = default);
}

public class PawTask2Service(HttpClient httpClient) : IPawTask2Service
{
    private const string FallbackBaseUrl = "https://jsonplaceholder.typicode.com/";

    public async Task<(IEnumerable<PawTask2DTO> Items, int TotalItems)> GetPawTask2ItemsAsync(int page = 1, int pageSize = 25, CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 25;

        try
        {
            var items = await httpClient.GetFromJsonAsync<List<PawTask2DTO>>("pawtask2items", ct);
            var list = items ?? new List<PawTask2DTO>();
            var total = list.Count;
            var paged = list.Skip((page - 1) * pageSize).Take(pageSize);
            return (paged, total);
        }
        catch (HttpRequestException)
        {
            using var fallbackClient = new HttpClient { BaseAddress = new Uri(FallbackBaseUrl) };
            var items = await fallbackClient.GetFromJsonAsync<List<PawTask2DTO>>("todos", ct);
            var list = items ?? new List<PawTask2DTO>();
            var total = list.Count;
            var paged = list.Skip((page - 1) * pageSize).Take(pageSize);
            return (paged, total);
        }
    }

    public async Task<PawTask2DTO?> GetPawTask2ItemByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<PawTask2DTO>($"pawtask2items/{id}", ct);
        }
        catch (HttpRequestException)
        {
            using var fallbackClient = new HttpClient { BaseAddress = new Uri(FallbackBaseUrl) };
            return await fallbackClient.GetFromJsonAsync<PawTask2DTO>($"todos/{id}", ct);
        }
    }

    public async Task<bool> CreatePawTask2Async(PawTask2DTO item, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("pawtask2items", item, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePawTask2Async(int id, PawTask2DTO item, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync($"pawtask2items/{id}", item, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePawTask2Async(int id, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"pawtask2items/{id}", ct);
        return response.IsSuccessStatusCode;
    }
}
