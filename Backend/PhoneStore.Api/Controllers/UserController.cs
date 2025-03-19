using PhoneStore.Domain.ModelResponse;

namespace PhoneStore.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;
        private readonly IRoleServices _roleServices;

        public UserController(IAccountServices accountServices, IMapper mapper, IRoleServices roleServices)
        {
            _accountServices = accountServices;
            _mapper = mapper;
            _roleServices = roleServices;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAccount()
        {
            var data = await _accountServices.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("GetById{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var statusResponse = new StatusResponse();
            var data = await _accountServices.GetByIdAsync(id);
            if (data == null)
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input not found";
                return NotFound(statusResponse);
            }
            var dataResponse = _mapper.Map<AccountResponse>(data);
            return Ok(dataResponse);
        }
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterAccount register)
        {
            var statusResponse = new StatusResponse();
            try
            {


                if (!ModelState.IsValid)
                {
                    statusResponse.status = -999;
                    statusResponse.mess = "Input not found";
                    return NotFound(statusResponse);
                }
                if (await CheckEmail(register.Email))
                {
                    statusResponse.status = 2;
                    statusResponse.mess = "Email is not valid";
                    return NotFound(statusResponse);
                }
                var defaultRole = (await _roleServices.GetAllAsync())
                            .FirstOrDefault(r => r.RoleName == "User");
                if (defaultRole is null)
                {
                    statusResponse.status = -1;
                    statusResponse.mess = "Default role not found!";
                    return BadRequest(statusResponse);
                }
                var dataAdd = _mapper.Map<Account>(register);
                dataAdd.NormalizedEmail = register.Email.ToUpper();
                dataAdd.Id = Guid.NewGuid();
                dataAdd.RoleId = defaultRole.Id;
                var result = await _accountServices.AddAsync(dataAdd);

                if (result > 0)
                {
                    statusResponse.status = 1;
                    statusResponse.mess = "Add successfully";
                    return Ok(statusResponse);
                }
                statusResponse.status = -2;
                statusResponse.mess = "Add fail";
                return BadRequest(statusResponse);
            }
            catch (Exception ex)
            {
                statusResponse.status = -999999;
                statusResponse.mess = ex.Message;
                return Ok(statusResponse);
                throw;
            }
        }
        [NonAction]
        public async Task<bool> CheckEmail(string email)
        {
            var data = await _accountServices.GetAllAsync();
            foreach (var item in data)
            {
                if (email.ToUpper() == item.NormalizedEmail)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
