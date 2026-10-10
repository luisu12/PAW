using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Key]
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("rating")]
    public int Rating { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static ProductDTO ConvertFrom(Product product)
    {
        // Guard against null inner values to avoid NullReferenceException when fields are missing
        return new ProductDTO
        {
            Id = product == null ? Guid.Empty : Guid.NewGuid(),
            ProductId = product?.ProductId ?? 0,
            Name = product?.ProductName ?? string.Empty,
            Description = product?.Description ?? string.Empty,
            Rating = (int)(product?.Rating ?? 0),
            ModifiedBy = product?.ModifiedBy,
            CreatedBy = product?.CreatedBy,
            Comments = product?.Comments ?? string.Empty,
            CreatedDate = product?.LastModified ?? DateTime.Now, // Assuming LastModified is used as CreatedDate
            ModifiedDate = product?.LastModified ?? DateTime.Now // Assuming LastModified is used as ModifiedDate
        };
    }

    public static Product ConvertTo(ProductDTO productDTO)
        {
            return new Product
            {
                ProductId = productDTO.ProductId,
                ProductName = productDTO.Name,
                Description = productDTO.Description,
                Rating = productDTO.Rating,
                Comments = productDTO.Comments,
                ModifiedBy = productDTO.ModifiedBy,
                CreatedBy = productDTO.CreatedBy,
                // Ensure LastModified is a valid SQL DateTime
                LastModified = productDTO.ModifiedDate == default(DateTime) ? DateTime.Now : productDTO.ModifiedDate
            };
    }
}
