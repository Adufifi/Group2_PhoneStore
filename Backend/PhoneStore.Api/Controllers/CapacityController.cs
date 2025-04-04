using Microsoft.AspNetCore.Authorization;

namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CapacityController : ControllerBase
    {
        private readonly ICapacityServices _capacityServices;
        private readonly IMapper _mapper;

        public CapacityController(ICapacityServices capacityServices, IMapper mapper)
        {
            _capacityServices = capacityServices;
            _mapper = mapper;
        }

        [HttpGet("All")]
        // [Authorize(policy: "asdfasdf")]
        public async Task<IActionResult> All()
        {

            var data = await _capacityServices.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("GetCapacityById/{id}")]
        public async Task<IActionResult> GetCapacityById(Guid id)
        {
            var data = await _capacityServices.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost("CreateCapacity")]
        public async Task<IActionResult> CreateCapacity([FromBody] CapacityDto capacityDto)
        {
            var dataAdd = _mapper.Map<Capacity>(capacityDto);
            dataAdd.CreatedDate = DateTime.Now;
            dataAdd.Id = Guid.NewGuid();
            var result = await _capacityServices.AddAsync(dataAdd);
            if (result > 0)
            {
                return Ok("Add success");
            }
            return StatusCode(500, "Add failed");
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteCapacity(Guid id)
        {
            var result = await _capacityServices.DeleteAsync(id);
            if (result)
            {
                return Ok("Delete success");
            }
            return NotFound("Delete failed");
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateCapacity(Guid id, CapacityDto capacityDto)
        {
            var dataUpdate = await _capacityServices.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                return NotFound("Capacity not found");
            }
            _mapper.Map(capacityDto, dataUpdate);
            var result = await _capacityServices.UpdateAsync(dataUpdate);
            if (result)
            {
                return Ok("Update success");
            }
            return StatusCode(500, "Update failed");
        }
    }
}