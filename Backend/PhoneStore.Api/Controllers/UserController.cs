

using System.Threading.Tasks;

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

        public UserController(IAccountServices accountServices
        , IMapper mapper
        , IRoleServices roleServices
        , IRefreshTokenServices refreshToken)
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
                dataAdd.Role = defaultRole;
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
            var authResult = new AuthResultVm();
            if (login == null)
            {
                authResult.status = -9999;
                authResult.mess = "Object null";
                return Ok(authResult);
            }
            if (!ModelState.IsValid)
            {
                authResult.status = -999;
                authResult.mess = "Input not valid";
                return Ok(authResult);
            }
            var emailExit = await _accountServices.GetAccountByEmail(login.Email);
            if (emailExit == null)
            {
                authResult.status = -2;
                authResult.mess = "Email not register";
                return Ok(authResult);
            }
            var checkPassword = _accountServices.CheckPassAccount(emailExit, login);
            if (!checkPassword)
            {
                authResult.status = 2;
                authResult.mess = "Password not correct";
                return Ok(authResult);
            }
            if (await checkIdAccountIsAdminOrUser(emailExit))
            {
                authResult = await _refreshToken.GenerateJwtToken(emailExit);
                authResult.status = 3;
                authResult.mess = "Account is Admin";
                return Ok(authResult);
            }
            else
            {
                authResult = await _refreshToken.GenerateJwtToken(emailExit);
                authResult.status = 1;
                authResult.mess = "Login successfully";
                return Ok(authResult);
            }


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
        [NonAction]
        async Task<bool> checkIdAccountIsAdminOrUser(Account account)
        {
            var role = await _roleServices.GetByIdAsync(account.RoleId);
            if (role.RoleName == "Admin")
            {
                return true;
            }
            return false;
        }
    }
}

