using System.ComponentModel.DataAnnotations;

namespace MagicTown_API.Modelos.DTO
{
    public class TownDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string ImagenURL { get; set; }

        public string Amenidad { get; set; }
    }
}
