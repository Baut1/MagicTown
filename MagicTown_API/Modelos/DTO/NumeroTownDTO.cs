using System.ComponentModel.DataAnnotations;

namespace MagicTown_API.Modelos.DTO
{
    public class NumeroTownDTO
    {
        [Required]
        public int TownNo { get; set; }

        [Required]
        public int TownId { get; set; }

        public string DetalleEspecial { get; set; }
    }
}
