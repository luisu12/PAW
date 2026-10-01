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
            GuidId = Guid.NewGuid(),
            Id = userRole.Id,
            RoldID = userRole.RoldID,
            UserID = userRole.UserID
        };
    }

    public static UserRole ConvertTo(UserRoleDTO userRoleDTO)
    {
        return new UserRole
        {
            Id = userRoleDTO.Id,
            RoldID = userRoleDTO.RoldID,
            UserID = userRoleDTO.UserID
        };
    }
}
