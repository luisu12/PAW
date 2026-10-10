using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Linq;
using PAW.Models;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<(IEnumerable<NotificationDTO> Items, int TotalItems)> GetNotificationsAsync(int page = 1, int pageSize = 25);
    Task<NotificationDTO?> GetNotificationByIdAsync(int id);
    Task<bool> CreateNotificationAsync(NotificationDTO notification);
    Task<bool> UpdateNotificationAsync(int id, NotificationDTO notification);
    Task<bool> DeleteNotificationAsync(int id);
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<(IEnumerable<NotificationDTO> Items, int TotalItems)> GetNotificationsAsync(int page = 1, int pageSize = 25)
    {
        var url = SetPathUrl(_path) + $"?page={page}&pageSize={pageSize}";
        var response = await _restProvider.GetAsync(url, id: null);
        try
        {
            var paged = await JsonProvider.DeserializeAsync<PAW.Models.PagedResultDTO<NotificationDTO>>(response);
            if (paged != null)
            {
                return (paged.Items ?? Enumerable.Empty<NotificationDTO>(), paged.TotalItems);
            }
        }
        catch (System.Text.Json.JsonException)
        {
            // fallback below
        }

        var items = await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
        var list = (items ?? Enumerable.Empty<NotificationDTO>()).ToList();
        var totalCount = list.Count;
        var pagedItems = list.Skip((page - 1) * pageSize).Take(pageSize);
        return (pagedItems, totalCount);
    }

    public async Task<NotificationDTO?> GetNotificationByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var notification = JsonProvider.DeserializeSimple<NotificationDTO>(response);
        return notification;
    }

    public async Task<bool> CreateNotificationAsync(NotificationDTO notification)
    {
        // API Save endpoint expects an array of Notification objects
        var content = JsonProvider.Serialize(new[] { notification });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> UpdateNotificationAsync(int id, NotificationDTO notification)
    {
        // API Save endpoint uses POST with an array for create/update
        var content = JsonProvider.Serialize(new[] { notification });
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return !string.IsNullOrEmpty(response);
    }

    public async Task<bool> DeleteNotificationAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return !string.IsNullOrEmpty(response);
    }
}
