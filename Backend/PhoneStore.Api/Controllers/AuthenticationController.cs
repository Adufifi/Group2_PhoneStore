namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        private readonly IRoleServices _roleServices;

        public AuthenticationController(IAccountServices acocuntServices, IRoleServices roleServices)
        {
            _accountServices = acocuntServices;
            _roleServices = roleServices;
        }
        [HttpPost("checkAdmin")]
        public async Task<IActionResult> Index([FromBody] CheckRequest checkRequest)
        {
            try
            {
                var statusResponse = new StatusResponse();
                if (string.IsNullOrEmpty(checkRequest.userId))
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "UserId is null or empty";
                    return BadRequest(statusResponse);
                }
                var account = await _accountServices.GetByIdAsync(Guid.Parse(checkRequest.userId));
                if (account == null)
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "Account not found";
                    return BadRequest(statusResponse);
                }
                var role = await _roleServices.GetByIdAsync(account.RoleId);
                if (role == null)
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "Role not found";
                    return BadRequest(statusResponse);
                }
                if (role.RoleName != "Admin")
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "User is not admin";
                    return BadRequest(statusResponse);
                }
                statusResponse.status = 200;
                statusResponse.mess = "User is admin";
                return Ok(statusResponse);
            }
            catch (Exception ex)
            {
                var statusResponse = new StatusResponse();
                statusResponse.status = 0;
                statusResponse.mess = ex.Message;
                return BadRequest(statusResponse);
            }
        }
        [HttpPost("checkUser")]
        public async Task<IActionResult> Index1([FromBody] CheckRequest checkRequest)
        {
            try
            {
                var statusResponse = new StatusResponse();
                if (string.IsNullOrEmpty(checkRequest.userId))
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "UserId is null or empty";
                    return BadRequest(statusResponse);
                }
                var account = await _accountServices.GetByIdAsync(Guid.Parse(checkRequest.userId));
                if (account == null)
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "Account not found";
                    return BadRequest(statusResponse);
                }
                var role = await _roleServices.GetByIdAsync(account.RoleId);
                if (role == null)
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "Role not found";
                    return BadRequest(statusResponse);
                }
                if (role.RoleName != "User")
                {
                    statusResponse.status = 0;
                    statusResponse.mess = "User is not admin";
                    return BadRequest(statusResponse);
                }
                statusResponse.status = 200;
                statusResponse.mess = "User is admin";
                return Ok(statusResponse);
            }
            catch (Exception ex)
            {
                var statusResponse = new StatusResponse();
                statusResponse.status = 0;
                statusResponse.mess = ex.Message;
                return BadRequest(statusResponse);
            }
        }
    }
}