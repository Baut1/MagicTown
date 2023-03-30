using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicTownAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaTowns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Towns",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenURL", "MetrosCuadrados", "Name", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "town detail...", new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(441), new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(429), "", 50, "Real Town", 5, 200.0 },
                    { 2, "", "town detail...", new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(444), new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(443), "", 40, "Premium Town", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
