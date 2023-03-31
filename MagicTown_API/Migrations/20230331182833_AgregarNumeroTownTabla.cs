using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicTownAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroTownTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroTowns",
                columns: table => new
                {
                    TownNo = table.Column<int>(type: "int", nullable: false),
                    TownId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroTowns", x => x.TownNo);
                    table.ForeignKey(
                        name: "FK_NumeroTowns_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 3, 31, 15, 28, 33, 627, DateTimeKind.Local).AddTicks(3819), new DateTime(2023, 3, 31, 15, 28, 33, 627, DateTimeKind.Local).AddTicks(3806) });

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 3, 31, 15, 28, 33, 627, DateTimeKind.Local).AddTicks(3821), new DateTime(2023, 3, 31, 15, 28, 33, 627, DateTimeKind.Local).AddTicks(3820) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroTowns_TownId",
                table: "NumeroTowns",
                column: "TownId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroTowns");

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(441), new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(429) });

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(444), new DateTime(2023, 3, 29, 20, 1, 43, 395, DateTimeKind.Local).AddTicks(443) });
        }
    }
}
