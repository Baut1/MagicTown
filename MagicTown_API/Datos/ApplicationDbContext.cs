using MagicTown_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicTown_API.Datos
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Town> Towns { get; set; }

        public DbSet<NumeroTown> NumeroTowns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>().HasData(
                new Town()
                {
                    Id = 1,
                    Name = "Real Town",
                    Detalle = "town detail...",
                    ImagenURL = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                },
                new Town()
                {
                    Id = 2,
                    Name = "Premium Town",
                    Detalle = "town detail...",
                    ImagenURL = "",
                    Ocupantes = 4,
                    MetrosCuadrados = 40,
                    Tarifa = 150,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                }
            );
        }
    }
}
