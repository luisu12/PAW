using System.Net.Http.Json;
using PAW.Models.DTO;

namespace PAW.MinimalAPI.Services;

public interface IPawTask2ExternalApiClient
{
    Task<IEnumerable<PawTask2DTO>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PawTask2DTO>> GetCompletedAsync(CancellationToken ct = default);
    Task<PawTask2DTO?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PawTask2DTO> CreateAsync(PawTask2DTO item, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, PawTask2DTO item, CancellationToken ct = default);
    Task<bool> PatchAsync(int id, PawTask2DTO item, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

public class PawTask2ExternalApiClient(HttpClient httpClient) : IPawTask2ExternalApiClient
{
    public async Task<IEnumerable<PawTask2DTO>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await httpClient.GetFromJsonAsync<List<PawTask2DTO>>("todos", ct);
        return items ?? Enumerable.Empty<PawTask2DTO>();
    }

    public async Task<IEnumerable<PawTask2DTO>> GetCompletedAsync(CancellationToken ct = default)
    {
        var all = await GetAllAsync(ct);
        return all.Where(x => x.Completed);
    }

    public async Task<PawTask2DTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await httpClient.GetFromJsonAsync<PawTask2DTO>($"todos/{id}", ct);
    }

    public async Task<PawTask2DTO> CreateAsync(PawTask2DTO item, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("todos", item, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PawTask2DTO>(ct)
            ?? item;
    }

    public async Task<bool> UpdateAsync(int id, PawTask2DTO item, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync($"todos/{id}", item, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PatchAsync(int id, PawTask2DTO item, CancellationToken ct = default)
    {
        var partial = new
        {
            title = item.Title,
            completed = item.Completed
        };

        var response = await httpClient.PatchAsJsonAsync($"todos/{id}", partial, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"todos/{id}", ct);
        return response.IsSuccessStatusCode;
    }
}
