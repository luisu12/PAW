using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserActionDTO
{
    [JsonPropertyName("Id")]
    public decimal Id { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    public static UserActionDTO ConvertFrom(UserAction action)
    {
        return new UserActionDTO
        {
            Id = action.Id ?? 0,
            Name = action.Name,
            Description = action.Description!
        };
    }

    public static UserAction ConvertTo(UserActionDTO actionDTO)
    {
        return new UserAction
        {
            Id = actionDTO.Id,
            Name = actionDTO.Name,
            Description = actionDTO.Description
        };
    }
}
