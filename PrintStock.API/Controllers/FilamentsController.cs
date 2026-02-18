using Microsoft.AspNetCore.Mvc;
using PrintStock.Application.DTOs;
using PrintStock.Application.Interfaces;

namespace PrintStock.API.Controllers;

[ApiController]
[Route("api/[controller]")] // Rota: api/filaments
public class FilamentsController : ControllerBase
{
    private readonly IFilamentService _filamentService;

    public FilamentsController(IFilamentService filamentService)
    {
        _filamentService = filamentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var filaments = await _filamentService.GetAllFilamentsAsync();
        return Ok(filaments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FilamentResponseDto dto) // [FromBody] ekledik!
    {
        if (dto == null) return BadRequest("Data is null");

        await _filamentService.AddFilamentAsync(dto);
        return Ok("Filament added successfully!");
    }
    [HttpPatch("{id}/add-stock")]
    public async Task<IActionResult> AddStock(Guid id, [FromBody] AdjustStockDto dto)
    {
        var result = await _filamentService.AddStockAsync(id, dto);
        return result ? Ok("Stock updated successfully!") : NotFound();
    }

    [HttpPatch("{id}/use")]
    public async Task<IActionResult> Use(Guid id, [FromBody] AdjustStockDto dto)
    {
        var result = await _filamentService.UseFilamentAsync(id, dto);
        return result ? Ok("Stock used successfully!") : BadRequest("Insufficient stock or not found.");
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _filamentService.DeleteFilamentAsync(id);
        return result ? Ok() : NotFound();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FilamentResponseDto dto)
    {
        var result = await _filamentService.UpdateFilamentAsync(id, dto);
        return result ? Ok("Updated successfully!") : NotFound();
    }
}
