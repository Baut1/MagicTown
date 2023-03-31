using MagicTown_API.Datos;
using MagicTown_API.Modelos;
using MagicTown_API.Repositorio.IRepositorio;

namespace MagicTown_API.Repositorio
{
    public class NumeroTownRepositorio : Repositorio<NumeroTown>, INumeroTownRepositorio
    {
        private readonly ApplicationDbContext _db;

        public NumeroTownRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<NumeroTown> Actualizar(NumeroTown entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroTowns.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
