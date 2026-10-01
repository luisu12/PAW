using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [Key]
    [JsonPropertyName("CategoryID")]
    public int CategoryID { get; set; }

    [JsonPropertyName("CategoryName")]
    public string? CategoryName { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("LastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("ModifiedBy")]
    public string? ModifiedBy { get; set; }

    public static CategoryDTO ConvertFrom(Category category)
    {
        return new CategoryDTO
        {
            Id = Guid.NewGuid(),
            CategoryID = category.CategoryID,
            CategoryName = category.CategoryName,
            Description = category.Description!,
            LastModified = category.LastModified ?? DateTime.Now,
            ModifiedBy = category.ModifiedBy
        };
    }

    public static Category ConvertTo(CategoryDTO categoryDTO)
    {
        return new Category
        {
            CategoryID = categoryDTO.CategoryID,
            CategoryName = categoryDTO.CategoryName,
            Description = categoryDTO.Description,
            LastModified = categoryDTO.LastModified,
            ModifiedBy = categoryDTO.ModifiedBy
        };
    }
}
