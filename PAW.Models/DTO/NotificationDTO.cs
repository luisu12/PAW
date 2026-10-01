using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class NotificationDTO
{
    [Key]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("is_read")]
    public bool? IsRead { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    public static NotificationDTO ConvertFrom(Notification notification)
    {
        return new NotificationDTO
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            IsRead = notification.IsRead ?? false,
            CreatedAt = notification.CreatedAt ?? DateTime.Now
        };
    }

    public static Notification ConvertTo(NotificationDTO notificationDTO)
    {
        return new Notification
        {
            Id = notificationDTO.Id,
            UserId = notificationDTO.UserId,
            Message = notificationDTO.Message,
            IsRead = notificationDTO.IsRead,
            CreatedAt = notificationDTO.CreatedAt
        };
    }
}
