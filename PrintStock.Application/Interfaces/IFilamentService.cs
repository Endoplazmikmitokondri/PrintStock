using PrintStock.Application.DTOs;

namespace PrintStock.Application.Interfaces;

public interface IFilamentService
{
    Task<List<FilamentResponseDto>> GetAllFilamentsAsync();
    Task<bool> UpdateFilamentAsync(Guid id, FilamentResponseDto dto);
    
    Task AddFilamentAsync(FilamentResponseDto dto);
    Task<bool> AddStockAsync(Guid id, AdjustStockDto dto);
    Task<bool> UseFilamentAsync(Guid id, AdjustStockDto dto);
    Task<bool> DeleteFilamentAsync(Guid id); 
}
