

namespace PhoneStore.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;
        private readonly IRoleServices _roleServices;
        private readonly IRefreshTokenServices _refreshToken;

        public UserController(IAccountServices accountServices, IMapper mapper, IRoleServices roleServices, IRefreshTokenServices refreshToken)
        {
            _accountServices = accountServices;
            _mapper = mapper;
            _roleServices = roleServices;
            _refreshToken = refreshToken;
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

                if (!IsValidEmail(register.Email))
                {
                    statusResponse.status = -909;
                    statusResponse.mess = "Email not valid";
                    return NotFound(statusResponse);
                }
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVm? login)
        {
            var statusResponse = new StatusResponse();
            if (login == null)
            {
                statusResponse.status = -9999;
                statusResponse.mess = "Object null";
                return Ok(statusResponse);
            }
            if (!ModelState.IsValid)
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input not valid";
                return Ok(statusResponse);
            }
            var emailExit = await _accountServices.GetAccountByEmail(login.Email);
            if (emailExit == null)
            {
                statusResponse.status = -2;
                statusResponse.mess = "Email chưa đăng ký";
                return Ok(statusResponse);
            }
            var checkPassword = _accountServices.CheckPassAccount(emailExit, login);
            if (checkPassword)
            {
                AuthResultVm authResult = await _refreshToken.GenerateJwtToken(emailExit);
                return Ok(authResult);
            }
            statusResponse.status = 2;
            statusResponse.mess = "Mật khẩu không đúng";
            return Ok(statusResponse);
        }
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var statusResponse = new StatusResponse();
            var result = await _accountServices.DeleteAsync(id);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Xoa thanh cong";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Xoa that bai";
            return Ok(statusResponse);
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
        [NonAction]
        static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

    }
}

