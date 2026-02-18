using PrintStock.Domain.Enums;

namespace PrintStock.Domain.Entities;

public class Filament
{
    public Guid Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    // Makaranın toplam kapasitesi (1kg, 5kg vb.)
    public double TotalWeight { get; set; }

    // Kalan miktar
    public double RemainingWeight { get; set; }

    // Nullable (boş geçilebilir) lokasyon bilgisi
    public string? StorageLocation { get; set; }

    public FilamentStatus Status { get; set; } = FilamentStatus.Sealed;
    public decimal? Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int? OptimalTemp { get; set; } // Nullable int
    public int? BedTemp { get; set; }
    public bool IsInDryBox { get; set; } = false; // Default false, kullanıcı dokunmazsa sorun yok
    public DateTime? OpenedDate { get; set; }
}
