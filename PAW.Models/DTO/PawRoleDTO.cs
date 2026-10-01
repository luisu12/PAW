using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class PawRoleDTO
{
    [Key]
    [JsonPropertyName("RoleID")]
    public int RoleID { get; set; }

    [JsonPropertyName("RoleName")]
    public string? RoleName { get; set; }

    public static PawRoleDTO ConvertFrom(PawRole role)
    {
        return new PawRoleDTO
        {
            Id = Guid.NewGuid(),
            RoleID = role.RoleID,
            RoleName = role.RoleName!
        };
    }

    public static PawRole ConvertTo(PawRoleDTO roleDTO)
    {
        return new PawRole
        {
            RoleID = roleDTO.RoleID,
            RoleName = roleDTO.RoleName
        };
    }
}
