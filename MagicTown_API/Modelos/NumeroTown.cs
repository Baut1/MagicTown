using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicTown_API.Modelos
{
    public class NumeroTown
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TownNo { get; set; }

        [Required]
        public int TownId { get; set; }

        [ForeignKey("TownId")]
        public Town Town { get; set; }

        public string DetalleEspecial { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
