namespace PhoneStore.Api.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        private readonly IMapper _mapper;

        public RoleController(IRoleServices roleServices, IMapper mapper)
        {
            _roleServices = roleServices;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            try
            {
                var data = await _roleServices.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var data = await _roleServices.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateAccount([FromBody] RoleDto roleDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return NotFound(statusResponse);
            }
            var roleExit = await _roleServices.GetAllAsync();
            foreach (var item in roleExit)
            {
                if (item.RoleName.ToUpper().Equals(roleDto.RoleName.ToUpper()))
                {
                    statusResponse.status = -909;
                    statusResponse.mess = "Role đã tồn tại";
                    return NotFound(statusResponse);
                }
            }
            var dataAdd = _mapper.Map<Role>(roleDto);
            dataAdd.CreatedDate = DateTime.Now;
            dataAdd.Id = Guid.NewGuid();
            var result = await _roleServices.AddAsync(dataAdd);
            if (result > 0)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Add success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Add valid";
            return StatusCode(500, statusResponse);
        }
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var statusResponse = new StatusResponse();
            var result = await _roleServices.DeleteAsync(id);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Delete success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Delete valid";
            return NotFound(statusResponse);
        }
        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, RoleDto roleDto)
        {
            var statusResponse = new StatusResponse();
            var dataUpdate = await _roleServices.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                statusResponse.status = -1;
                statusResponse.mess = "Role not found";
                return NotFound(statusResponse);
            }
            if (!ModelState.IsValid)
            {
                statusResponse.status = -909;
                statusResponse.mess = "Input invalid";
                return BadRequest(statusResponse);
            }
            _mapper.Map(roleDto, dataUpdate);
            var result = await _roleServices.UpdateAsync(dataUpdate);
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
