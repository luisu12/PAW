using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserRoleDTO
{
    [JsonPropertyName("Id")]
    public decimal Id { get; set; }

    [JsonPropertyName("RoldID")]
    public decimal? RoldID { get; set; }

    [JsonPropertyName("UserID")]
    public decimal? UserID { get; set; }

    public static UserRoleDTO ConvertFrom(UserRole userRole)
    {
        return new UserRoleDTO
        {
            Id = userRole.Id ?? 0,
            RoldID = userRole.RoldId,
            UserID = userRole.UserId
        };
    }

    public static UserRole ConvertTo(UserRoleDTO userRoleDTO)
    {
        return new UserRole
        {
            Id = userRoleDTO.Id,
            RoldId = userRoleDTO.RoldID,
            UserId = userRoleDTO.UserID
        };
    }
}
