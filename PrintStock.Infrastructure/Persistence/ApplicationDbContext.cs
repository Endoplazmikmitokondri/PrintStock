using Microsoft.EntityFrameworkCore;
using PrintStock.Domain.Entities;
using PrintStock.Application.Interfaces;

namespace PrintStock.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Envanter tablomuz
    public DbSet<Filament> Filaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // SQLite'ta Id alanını anahtar olarak belirliyoruz.
        // SQL Server'daki gibi özel kolon tipi (decimal vb.) belirtmeye gerek yok, 
        // SQLite bunları dinamik olarak yönetir.
        modelBuilder.Entity<Filament>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }

    // Arayüzden gelen SaveChangesAsync metodunu DbContext'in kendi metoduyla eşliyoruz
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
