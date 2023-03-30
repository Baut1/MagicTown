using AutoMapper;
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
        private readonly IMapper _mapper;

        public TownController(ILogger<TownController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TownDTO>>> GetTowns()
        {
            _logger.LogInformation("Obtain the towns");

            IEnumerable<Town> townList = await _db.Towns.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<TownDTO>>(townList));
        }

        [HttpGet("id:int", Name="GetTown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TownDTO>> GetTown(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error when retrieving town with id: " + id);
                return BadRequest();
            }

            var town = await _db.Towns.FirstOrDefaultAsync(t =>  t.Id == id);

            if (town == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TownDTO>(town));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TownDTO>> CreateTown([FromBody] TownCreateDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _db.Towns.FirstOrDefaultAsync(t => t.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameExists", "The town with that name already exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            Town modelo = _mapper.Map<Town>(createDTO);

            await _db.Towns.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetTown", new {id=modelo.Id}, modelo);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTown(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var town = await _db.Towns.FirstOrDefaultAsync(t => t.Id == id);

            if (town == null)
            {
                return NotFound();
            }

            _db.Towns.Remove(town);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> UpdateTown(int id, [FromBody] TownUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }

            Town modelo = _mapper.Map<Town>(updateDTO);
            
            _db.Towns.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialTown(int id, JsonPatchDocument<TownUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var town = await _db.Towns.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            TownUpdateDTO townDTO = _mapper.Map<TownUpdateDTO>(town);

            if (town == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(townDTO, ModelState);

            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Town modelo = _mapper.Map<Town>(townDTO);

            _db.Towns.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
