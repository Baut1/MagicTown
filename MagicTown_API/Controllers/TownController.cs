﻿using AutoMapper;
using MagicTown_API.Modelos;
using MagicTown_API.Modelos.DTO;
using MagicTown_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicTown_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroTownController : ControllerBase
    {
        private readonly ILogger<NumeroTownController> _logger;
        private readonly ITownRepositorio _townRepo;
        private readonly INumeroTownRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroTownController(ILogger<NumeroTownController> logger, ITownRepositorio townRepo, INumeroTownRepositorio numeroRepo, IMapper mapper)
        {
            _logger = logger;
            _townRepo = townRepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroTowns()
        {
            try
            {
                _logger.LogInformation("Obtain the towns");

                IEnumerable<NumeroTown> numeroTownList = await _numeroRepo.ObtenerTodos();

                _response.Result = _mapper.Map<IEnumerable<TownDTO>>(numeroTownList);
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

        [HttpGet("id:int", Name="GetNumeroTown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroTown(int id)
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

                var numeroTown = await _numeroRepo.Obtener(t => t.TownNo == id);

                if (numeroTown == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful=false;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<NumeroTownDTO>(numeroTown);
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
        public async Task<ActionResult<APIResponse>> CreateNumeroTown([FromBody] NumeroTownCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _numeroRepo.Obtener(t => t.TownNo == createDTO.TownNo) != null)
                {
                    ModelState.AddModelError("NameExists", "The town with that name already exists!");
                    return BadRequest(ModelState);
                }

                if (await _townRepo.Obtener(t => t.Id == createDTO.TownId) == null)
                {
                    ModelState.AddModelError("ForeignKey", "The town with that name already exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                NumeroTown modelo = _mapper.Map<NumeroTown>(createDTO);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _numeroRepo.Crear(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroTown", new { id = modelo.TownNo }, _response);
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
        public async Task<IActionResult> DeleteNumeroTown(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var numeroTown = await _numeroRepo.Obtener(t => t.TownNo == id);
                if (numeroTown == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _numeroRepo.Remover(numeroTown);

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
        public async Task<IActionResult> UpdateNumeroTown(int id, [FromBody] NumeroTownUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.TownNo)
            {
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _townRepo.Obtener(t => t.Id == updateDTO.TownId) == null)
            {
                ModelState.AddModelError("ForeignKey", "Id no existe");
                return BadRequest(ModelState);
            }

            NumeroTown modelo = _mapper.Map<NumeroTown>(updateDTO);
            
            await _numeroRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
