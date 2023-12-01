using AutoMapper;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dto;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _villaNumberRepo;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;
        public VillaNumberAPIController(IVillaNumberRepository villaNumberRepo, IMapper mapper)
        {
            _villaNumberRepo = villaNumberRepo;
            _apiResponse = new APIResponse();
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            var villaList = await _villaNumberRepo.GetAllAsync();
            _apiResponse.Result = _mapper.Map<List<VillaNumberDTO>>(villaList);
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            if (villaNo == 0)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }
            var villaNumber = await _villaNumberRepo.GetAsync(x => x.VillaNo == villaNo);
            if (villaNumber == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaNumberDTO villaNumberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }
            if (villaNumberDto == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            if (await _villaNumberRepo.GetAsync(x => x.VillaNo == villaNumberDto.VillaNo) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Number already exists!");
                return BadRequest(ModelState);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDto);
            await _villaNumberRepo.CreateAsync(model);

            _apiResponse.Result = model;
            _apiResponse.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = model.VillaNo }, _apiResponse);
        }
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            if (villaNo == 0)
            {
                return BadRequest();
            }
            var villaNumber = await _villaNumberRepo.GetAsync(x => x.VillaNo == villaNo);
            if (villaNumber == null)
            {
                return NotFound();
            }
            await _villaNumberRepo.RemoveAsync(villaNumber);
            _apiResponse.StatusCode = HttpStatusCode.NoContent;
            return Ok(_apiResponse);
        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaNumberDTO villaNumberDto)
        {
            if (villaNumberDto == null || id != villaNumberDto.VillaNo)
            {
                return BadRequest();
            }
            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDto);
            await _villaNumberRepo.UpdateAsync(model);
            _apiResponse.StatusCode = HttpStatusCode.NoContent;
            return Ok(_apiResponse);
        }
    }
}
