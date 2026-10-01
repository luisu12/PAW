using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ComponentDTO
{
    [JsonPropertyName("ID")]
    public decimal ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;

    public static ComponentDTO ConvertFrom(Component component)
    {
        return new ComponentDTO
        {
            ID = component.Id,
            Name = component.Name,
            Content = component.Content
        };
    }

    public static Component ConvertTo(ComponentDTO componentDTO)
    {
        return new Component
        {
            Id = componentDTO.ID,
            Name = componentDTO.Name,
            Content = componentDTO.Content
        };
    }
}
