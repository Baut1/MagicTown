using MagicTown_API.Datos;
using MagicTown_API.Modelos;
using MagicTown_API.Repositorio.IRepositorio;

namespace MagicTown_API.Repositorio
{
    public class TownRepositorio : Repositorio<Town>, ITownRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TownRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<Town> Actualizar(Town entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.Towns.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
