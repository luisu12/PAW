using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
    [Key]
    [JsonPropertyName("UserID")]
    public int UserID { get; set; }

    [JsonPropertyName("Username")]
    public string? Username { get; set; }

    [JsonPropertyName("Email")]
    public string? Email { get; set; }

    [JsonPropertyName("PasswordHash")]
    public string? PasswordHash { get; set; }

    [JsonPropertyName("CreatedAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("IsActive")]
    public bool? IsActive { get; set; }

    [JsonPropertyName("LastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("ModifiedBy")]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("RoleID")]
    public int? RoleID { get; set; }

    [JsonPropertyName("LastModifiedBy")]
    public string? LastModifiedBy { get; set; }

    public static UserDTO ConvertFrom(User user)
    {
        return new UserDTO
        {
            Id = Guid.NewGuid(),
            UserID = user.UserID,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            CreatedAt = user.CreatedAt ?? DateTime.Now,
            IsActive = user.IsActive ?? true,
            LastModified = user.LastModified ?? DateTime.Now,
            ModifiedBy = user.ModifiedBy,
            RoleID = user.RoleID,
            LastModifiedBy = user.LastModifiedBy
        };
    }

    public static User ConvertTo(UserDTO userDTO)
    {
        return new User
        {
            UserID = userDTO.UserID,
            Username = userDTO.Username,
            Email = userDTO.Email,
            PasswordHash = userDTO.PasswordHash,
            CreatedAt = userDTO.CreatedAt,
            IsActive = userDTO.IsActive,
            LastModified = userDTO.LastModified,
            ModifiedBy = userDTO.ModifiedBy,
            RoleID = userDTO.RoleID,
            LastModifiedBy = userDTO.LastModifiedBy
        };
    }
}
