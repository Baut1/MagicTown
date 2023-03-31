using AutoMapper;
using MagicTown_API.Datos;
using MagicTown_API.Modelos;
using MagicTown_API.Modelos.DTO;
using MagicTown_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicTown_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly ILogger<TownController> _logger;
        private readonly ITownRepositorio _townRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public TownController(ILogger<TownController> logger, ITownRepositorio townRepo, IMapper mapper)
        {
            _logger = logger;
            _townRepo = townRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetTowns()
        {
            try
            {
                _logger.LogInformation("Obtain the towns");

                IEnumerable<Town> townList = await _townRepo.ObtenerTodos();

                _response.Result = _mapper.Map<IEnumerable<TownDTO>>(townList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>(){ ex.ToString() };
            }

            return _response;
        }

        [HttpGet("id:int", Name="GetTown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTown(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error when retrieving town with id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful=false;
                    return BadRequest(_response);
                }

                var town = await _townRepo.Obtener(t => t.Id == id);

                if (town == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful=false;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<TownDTO>(town);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTown([FromBody] TownCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _townRepo.Obtener(t => t.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("NameExists", "The town with that name already exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Town modelo = _mapper.Map<Town>(createDTO);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _townRepo.Crear(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTown", new { id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTown(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var town = await _townRepo.Obtener(t => t.Id == id);
                if (town == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _townRepo.Remover(town);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful=false;
                _response.ErrorMessages = new List<string>(){ ex.ToString() };
            }

            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> UpdateTown(int id, [FromBody] TownUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Town modelo = _mapper.Map<Town>(updateDTO);
            
            await _townRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
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

            var town = await _townRepo.Obtener(t => t.Id == id, tracked: false);

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

            await _townRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
