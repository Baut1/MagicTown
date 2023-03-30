using MagicTown_API.Datos;
using MagicTown_API.Modelos;
using MagicTown_API.Modelos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicTown_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly ILogger<TownController> _logger;
        private readonly ApplicationDbContext _db;

        public TownController(ILogger<TownController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<TownDTO>> GetTowns()
        {
            _logger.LogInformation("Obtain the towns");
            return Ok(_db.Towns.ToList());
        }

        [HttpGet("id:int", Name="GetTown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TownDTO> GetTown(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error when retrieving town with id: " + id);
                return BadRequest();
            }

            var town = _db.Towns.FirstOrDefault(t =>  t.Id == id);

            if (town == null)
            {
                return NotFound();
            }

            return Ok(town);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TownDTO> CreateTown([FromBody] TownDTO townDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_db.Towns.FirstOrDefault(t => t.Name.ToLower() == townDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameExists", "The town with that name already exists!");
                return BadRequest(ModelState);
            }

            if (townDTO == null)
            {
                return BadRequest(townDTO);
            }
            if (townDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Town modelo = new()
            {
                Name = townDTO.Name,
                Detalle = townDTO.Detalle,
                ImagenURL = townDTO.ImagenURL,
                Ocupantes = townDTO.Ocupantes,
                Tarifa = townDTO.Tarifa,
                MetrosCuadrados = townDTO.MetrosCuadrados,
                Amenidad = townDTO.Amenidad,
            };

            _db.Towns.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetTown", new {id=townDTO.Id}, townDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTown(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var town = _db.Towns.FirstOrDefault(t => t.Id == id);

            if (town == null)
            {
                return NotFound();
            }

            _db.Towns.Remove(town);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public IActionResult UpdateTown(int id, [FromBody] TownDTO townDTO)
        {
            if (townDTO == null || id != townDTO.Id)
            {
                return BadRequest();
            }

            Town modelo = new()
            {
                Id = townDTO.Id,
                Name = townDTO.Name,
                Detalle = townDTO.Detalle,
                ImagenURL = townDTO.ImagenURL,
                Ocupantes = townDTO.Ocupantes,
                Tarifa = townDTO.Tarifa,
                MetrosCuadrados = townDTO.MetrosCuadrados,
                Amenidad = townDTO.Amenidad,
            };
            
            _db.Towns.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialTown(int id, JsonPatchDocument<TownDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var town = _db.Towns.AsNoTracking().FirstOrDefault(t => t.Id == id);

            TownDTO townDTO = new()
            {
                Id = town.Id,
                Name = town.Name,
                Detalle = town.Detalle,
                ImagenURL = town.ImagenURL,
                Ocupantes = town.Ocupantes,
                Tarifa = town.Tarifa,
                MetrosCuadrados = town.MetrosCuadrados,
                Amenidad = town.Amenidad
            };

            if (town == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(townDTO, ModelState);

            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Town modelo = new()
            {
                Id = townDTO.Id,
                Name = townDTO.Name,
                Detalle = townDTO.Detalle,
                ImagenURL = townDTO.ImagenURL,
                Ocupantes = townDTO.Ocupantes,
                Tarifa = townDTO.Tarifa,
                MetrosCuadrados = townDTO.MetrosCuadrados,
                Amenidad = townDTO.Amenidad,
            };

            _db.Towns.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
