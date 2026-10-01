using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class SupplierDTO
{
    [Key]
    [JsonPropertyName("SupplierID")]
    public int SupplierID { get; set; }

    [JsonPropertyName("SupplierName")]
    public string? SupplierName { get; set; }

    [JsonPropertyName("ContactName")]
    public string? ContactName { get; set; }

    [JsonPropertyName("ContactTitle")]
    public string? ContactTitle { get; set; }

    [JsonPropertyName("Phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("Address")]
    public string? Address { get; set; }

    [JsonPropertyName("City")]
    public string? City { get; set; }

    [JsonPropertyName("Country")]
    public string? Country { get; set; }

    [JsonPropertyName("LastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("ModifiedBy")]
    public string? ModifiedBy { get; set; }

    public static SupplierDTO ConvertFrom(Supplier supplier)
    {
        return new SupplierDTO
        {
            Id = Guid.NewGuid(),
            SupplierID = supplier.SupplierID,
            SupplierName = supplier.SupplierName,
            ContactName = supplier.ContactName,
            ContactTitle = supplier.ContactTitle,
            Phone = supplier.Phone,
            Address = supplier.Address,
            City = supplier.City,
            Country = supplier.Country,
            LastModified = supplier.LastModified ?? DateTime.Now,
            ModifiedBy = supplier.ModifiedBy
        };
    }

    public static Supplier ConvertTo(SupplierDTO supplierDTO)
    {
        return new Supplier
        {
            SupplierID = supplierDTO.SupplierID,
            SupplierName = supplierDTO.SupplierName,
            ContactName = supplierDTO.ContactName,
            ContactTitle = supplierDTO.ContactTitle,
            Phone = supplierDTO.Phone,
            Address = supplierDTO.Address,
            City = supplierDTO.City,
            Country = supplierDTO.Country,
            LastModified = supplierDTO.LastModified,
            ModifiedBy = supplierDTO.ModifiedBy
        };
    }
}
