using System.Net.Http.Json;
using PrintStock.Application.DTOs;

namespace PrintStock.UI.Services;

public class FilamentHttpClient
{
    private readonly HttpClient _http;

    public FilamentHttpClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<FilamentResponseDto>> GetFilamentsAsync()
    {
        return await _http.GetFromJsonAsync<List<FilamentResponseDto>>("api/filaments") ?? new();
    }

    // --- YENİ: DÜZENLEME (UPDATE) ÖZELLİĞİ ---
    public async Task<bool> UpdateFilamentAsync(Guid id, FilamentResponseDto dto)
    {
        // Controller'daki [HttpPut("{id}")] endpoint'ine gider
        var response = await _http.PutAsJsonAsync($"api/filaments/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddStockAsync(Guid id, AdjustStockDto dto)
    {
        var response = await _http.PatchAsJsonAsync($"api/filaments/{id}/add-stock", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UseStockAsync(Guid id, AdjustStockDto dto)
    {
        var response = await _http.PatchAsJsonAsync($"api/filaments/{id}/use", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateFilamentAsync(FilamentResponseDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/filaments", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteFilamentAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/filaments/{id}");
        return response.IsSuccessStatusCode;
    }
}
