using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();
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

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var notifications = await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
        return notifications;
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
