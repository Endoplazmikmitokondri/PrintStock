using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialType = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    TotalWeight = table.Column<double>(type: "REAL", nullable: false),
                    RemainingWeight = table.Column<double>(type: "REAL", nullable: false),
                    StorageLocation = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OptimalTemp = table.Column<int>(type: "INTEGER", nullable: true),
                    BedTemp = table.Column<int>(type: "INTEGER", nullable: true),
                    IsInDryBox = table.Column<bool>(type: "INTEGER", nullable: false),
                    OpenedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filaments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filaments");
        }
    }
}
