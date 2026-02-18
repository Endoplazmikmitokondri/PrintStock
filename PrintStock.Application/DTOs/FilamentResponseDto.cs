using System.ComponentModel.DataAnnotations;
using PrintStock.Domain.Enums;

namespace PrintStock.Application.DTOs;

public class FilamentResponseDto
{
    public Guid Id { get; set; }

    [Required, StringLength(50, ErrorMessage = "Brand name too long (max 50).")]
    public string Brand { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string MaterialType { get; set; } = string.Empty;

    [StringLength(30)]
    public string Color { get; set; } = string.Empty;

    [Range(0, 100000, ErrorMessage = "Weight must be between 0 and 100kg.")]
    public double TotalWeight { get; set; }

    [Range(0, 100000)]
    public double RemainingWeight { get; set; }

    [StringLength(50, ErrorMessage = "Location too long (max 50).")]
    public string? StorageLocation { get; set; }

    public FilamentStatus Status { get; set; }
    public DateTime PurchaseDate { get; set; }

    [Range(0, 1000000, ErrorMessage = "Invalid price range.")]
    public decimal? Price { get; set; }

    [Range(0, 500, ErrorMessage = "Nozzle temp must be 0-500°C.")]
    public int? OptimalTemp { get; set; }

    [Range(0, 200, ErrorMessage = "Bed temp must be 0-200°C.")]
    public int? BedTemp { get; set; }

    public bool IsInDryBox { get; set; }
    public DateTime? OpenedDate { get; set; }

    public decimal CostPerGram => (Price.HasValue && TotalWeight > 0) ? Price.Value / (decimal)TotalWeight : 0;
    public decimal CurrentStockValue => (decimal)RemainingWeight * CostPerGram;

    public string WeightDisplay => RemainingWeight >= 1000 
        ? $"{(RemainingWeight / 1000):N2} kg" 
        : $"{RemainingWeight} g";

    public string StatusColor => Status switch
    {
        FilamentStatus.Active => "success",
        FilamentStatus.Sealed => "primary",
        FilamentStatus.Finished => "secondary",
        _ => "light"
    };
}
