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
            Id = Guid.NewGuid(),
            ID = component.ID,
            Name = component.name,
            Content = component.content!
        };
    }

    public static Component ConvertTo(ComponentDTO componentDTO)
    {
        return new Component
        {
            ID = componentDTO.ID,
            name = componentDTO.Name,
            content = componentDTO.Content
        };
    }
}
