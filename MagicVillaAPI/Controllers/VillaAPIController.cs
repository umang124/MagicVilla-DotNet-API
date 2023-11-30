using AutoMapper;
using MagicVillaAPI.Data;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dto;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;
        public VillaAPIController( IMapper mapper, IVillaRepository villaRepo)
        {         
            _villaRepo = villaRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            var villaList = await _villaRepo.GetAllAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _villaRepo.GetAsync(x => x.Id == id, false);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaDTO villaDto)
        {
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (await _villaRepo.GetAsync(x => x.VillaName.ToLower() == villaDto.VillaName.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            }

            Villa model = _mapper.Map<Villa>(villaDto);

            await _villaRepo.CreateAsync(model);
            return CreatedAtRoute("GetVilla", new { id = model.Id }, villaDto);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _villaRepo.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            await _villaRepo.RemoveAsync(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaDTO villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            Villa model = _mapper.Map<Villa>(villaDto);
            await _villaRepo.UpdateAsync(model);

            return NoContent();
        }
    }
}
