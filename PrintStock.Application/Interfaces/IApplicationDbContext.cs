using Microsoft.EntityFrameworkCore;
using PrintStock.Domain.Entities;

namespace PrintStock.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Filament> Filaments { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
