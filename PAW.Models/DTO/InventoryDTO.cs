using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class InventoryDTO
{
    [Key]
    [JsonPropertyName("InventoryID")]
    public int InventoryID { get; set; }

    [JsonPropertyName("UnitPrice")]
    public decimal? UnitPrice { get; set; }

    [JsonPropertyName("UnitsInStock")]
    public int? UnitsInStock { get; set; }

    [JsonPropertyName("LastUpdated")]
    public DateTime? LastUpdated { get; set; }

    [JsonPropertyName("ProductId")]
    public int? ProductId { get; set; }

    [JsonPropertyName("DateAdded")]
    public DateTime? DateAdded { get; set; }

    [JsonPropertyName("ModifiedBy")]
    public string? ModifiedBy { get; set; }

    public static InventoryDTO ConvertFrom(Inventory inventory)
    {
        return new InventoryDTO
        {
            InventoryID = inventory.InventoryId,
            UnitPrice = inventory.UnitPrice,
            UnitsInStock = inventory.UnitsInStock,
            LastUpdated = inventory.LastUpdated ?? DateTime.Now,
            ProductId = inventory.ProductId,
            DateAdded = inventory.DateAdded ?? DateTime.Now,
            ModifiedBy = inventory.ModifiedBy
        };
    }

    public static Inventory ConvertTo(InventoryDTO inventoryDTO)
    {
        return new Inventory
        {
            InventoryId = inventoryDTO.InventoryID,
            UnitPrice = inventoryDTO.UnitPrice,
            UnitsInStock = inventoryDTO.UnitsInStock,
            LastUpdated = inventoryDTO.LastUpdated,
            ProductId = inventoryDTO.ProductId,
            DateAdded = inventoryDTO.DateAdded,
            ModifiedBy = inventoryDTO.ModifiedBy
        };
    }
}
