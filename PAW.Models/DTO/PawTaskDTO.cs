using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class PawTaskDTO
{
    [Key]
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    [JsonPropertyName("DueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("CreatedAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("LastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("ModifiedBy")]
    public string? ModifiedBy { get; set; }

    public static PawTaskDTO ConvertFrom(PawTask task)
    {
        return new PawTaskDTO
        {
            GuidId = Guid.NewGuid(),
            Id = task.Id,
            Name = task.Name,
            Description = task.Description!,
            Status = task.Status,
            DueDate = task.DueDate ?? DateTime.Now,
            CreatedAt = task.CreatedAt ?? DateTime.Now,
            LastModified = task.LastModified ?? DateTime.Now,
            ModifiedBy = task.ModifiedBy
        };
    }

    public static PawTask ConvertTo(PawTaskDTO taskDTO)
    {
        return new PawTask
        {
            Id = taskDTO.Id,
            Name = taskDTO.Name,
            Description = taskDTO.Description,
            Status = taskDTO.Status,
            DueDate = taskDTO.DueDate,
            CreatedAt = taskDTO.CreatedAt,
            LastModified = taskDTO.LastModified,
            ModifiedBy = taskDTO.ModifiedBy
        };
    }
}
