using MagicTown_API.Modelos;

namespace MagicTown_API.Repositorio.IRepositorio
{
    public interface ITownRepositorio: IRepositorio<Town>
    {
        Task<Town> Actualizar(Town entidad);
    }
}
