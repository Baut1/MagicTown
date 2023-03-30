using MagicTown_API.Modelos.DTO;

namespace MagicTown_API.Datos
{
    public static class TownStore
    {
        public static List<TownDTO> townList = new List<TownDTO>
        {
            new TownDTO{ Id = 1, Name = "vista a la piscina", Ocupantes=3, MetrosCuadrados=50 },
            new TownDTO{ Id = 2, Name = "vista a la playa", Ocupantes=4, MetrosCuadrados=80 }
        };
    }
}
