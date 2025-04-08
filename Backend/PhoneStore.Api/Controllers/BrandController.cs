using Microsoft.AspNetCore.Authorization;

namespace PhoneStore.Api.Controllers
{
    [Route("api/brand")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandServices _brandService;
        private readonly IMapper _mapper;
        public BrandController(IBrandServices brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }
        [HttpGet("AllBrand")]
        public async Task<IActionResult> AllBrand()
        {
            var data = await _brandService.GetAllAsync();
            var orderedData = data.OrderBy(x => x.CreatedDate).ToList();
            return Ok(orderedData);
        }
        [HttpGet("GetBrandById/{id}")]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var data = await _brandService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpPost("CreateBrand")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandDto brandDto)
        {
            var statusResponse = new StatusResponse();

            if (!ModelState.IsValid)
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return BadRequest(statusResponse);
            }

            var dataAdd = _mapper.Map<Brand>(brandDto);
            dataAdd.CreatedDate = DateTime.Now;
            dataAdd.Id = Guid.NewGuid();

            var result = await _brandService.AddAsync(dataAdd);

            if (result > 0)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Add success";
                return Ok(statusResponse);
            }

            statusResponse.status = -2;
            statusResponse.mess = "Failed to add brand";
            return StatusCode(500, statusResponse);
        }
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            var statusResponse = new StatusResponse();
            var result = await _brandService.DeleteAsync(id);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Delete success";
                return Ok(statusResponse);
            }

            statusResponse.status = -2;
            statusResponse.mess = "Delete valid";
            return StatusCode(500, statusResponse);
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateBrand(Guid id, BrandDto brandDto)
        {
            var statusResponse = new StatusResponse();


            var dataUpdate = await _brandService.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                statusResponse.status = -1;
                statusResponse.mess = "Brand not found";
                return NotFound(statusResponse);
            }


            if (!ModelState.IsValid)
            {
                statusResponse.status = -909;
                statusResponse.mess = "Input invalid";
                return BadRequest(statusResponse);
            }

            _mapper.Map(brandDto, dataUpdate);

            var result = await _brandService.UpdateAsync(dataUpdate);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Update success";
                return Ok(statusResponse);
            }

            statusResponse.status = -2;
            statusResponse.mess = "Update valid";
            return StatusCode(500, statusResponse);
        }

    }
}
