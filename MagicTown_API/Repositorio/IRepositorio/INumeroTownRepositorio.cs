using MagicTown_API.Modelos;

namespace MagicTown_API.Repositorio.IRepositorio
{
    public interface INumeroTownRepositorio: IRepositorio<NumeroTown>
    {
        Task<NumeroTown> Actualizar(NumeroTown entidad);
    }
}
