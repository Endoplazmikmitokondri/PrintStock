using Microsoft.EntityFrameworkCore;
using PrintStock.Application.DTOs;
using PrintStock.Application.Interfaces;
using PrintStock.Domain.Entities;
using PrintStock.Domain.Enums;

namespace PrintStock.Application.Services;

public class FilamentService : IFilamentService
{
    private readonly IApplicationDbContext _context;

    public FilamentService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FilamentResponseDto>> GetAllFilamentsAsync()
    {
        return await _context.Filaments
            .Select(f => new FilamentResponseDto
            {
                Id = f.Id,
                Brand = f.Brand,
                MaterialType = f.MaterialType,
                Color = f.Color,
                RemainingWeight = f.RemainingWeight,
                TotalWeight = f.TotalWeight,
                Status = f.Status,
                StorageLocation = f.StorageLocation,
                PurchaseDate = f.PurchaseDate,
                OptimalTemp = f.OptimalTemp,
                BedTemp = f.BedTemp,
                Price = f.Price,
                IsInDryBox = f.IsInDryBox,
                OpenedDate = f.OpenedDate
            }).ToListAsync();
    }

    public async Task<bool> UseFilamentAsync(Guid id, AdjustStockDto dto)
    {
        var filament = await _context.Filaments.FindAsync(id);
        if (filament == null) return false;

        double amountInGrams = dto.Unit == WeightUnit.Kilogram ? dto.Amount * 1000 : dto.Amount;
        if (filament.RemainingWeight < amountInGrams) return false;

        // Statüyü Aktif yap
        if (filament.Status == FilamentStatus.Sealed)
            filament.Status = FilamentStatus.Active;

        filament.RemainingWeight -= amountInGrams;

        // --- KRİTİK EKLEME: BİTİŞ KONTROLÜ ---
        if (filament.RemainingWeight <= 0)
        {
            filament.RemainingWeight = 0; // Negatife düşmemesi için sabitle
            filament.Status = FilamentStatus.Finished; // Gri (secondary) olacak
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task AddFilamentAsync(FilamentResponseDto dto)
    {
        var filament = new Filament
        {
            Id = Guid.NewGuid(),
            Brand = dto.Brand,
            MaterialType = dto.MaterialType,
            Color = dto.Color,
            TotalWeight = dto.TotalWeight,
            RemainingWeight = dto.TotalWeight,
            StorageLocation = dto.StorageLocation,
            Status = FilamentStatus.Sealed,
            PurchaseDate = dto.PurchaseDate,
            Price = dto.Price,
            OptimalTemp = dto.OptimalTemp,
            BedTemp = dto.BedTemp,
            IsInDryBox = dto.IsInDryBox,
            OpenedDate = dto.OpenedDate
        };

        _context.Filaments.Add(filament);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AddStockAsync(Guid id, AdjustStockDto dto)
    {
        var filament = await _context.Filaments.FindAsync(id);
        if (filament == null) return false;

        double amountInGrams = dto.Unit == WeightUnit.Kilogram ? dto.Amount * 1000 : dto.Amount;

        filament.RemainingWeight += amountInGrams;

        if (filament.Status == FilamentStatus.Finished && filament.RemainingWeight > 0)
        {
            filament.Status = FilamentStatus.Active;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFilamentAsync(Guid id)
    {
        var filament = await _context.Filaments.FindAsync(id);
        if (filament == null) return false;

        _context.Filaments.Remove(filament);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> UpdateFilamentAsync(Guid id, FilamentResponseDto dto)
    {
        var filament = await _context.Filaments.FindAsync(id);
        if (filament == null) return false;

        filament.Brand = dto.Brand;
        filament.MaterialType = dto.MaterialType;
        filament.Color = dto.Color;
        filament.Price = dto.Price;
        filament.OptimalTemp = dto.OptimalTemp;
        filament.BedTemp = dto.BedTemp;
        filament.StorageLocation = dto.StorageLocation;
        filament.IsInDryBox = dto.IsInDryBox;

        await _context.SaveChangesAsync();
        return true;
    }
}
