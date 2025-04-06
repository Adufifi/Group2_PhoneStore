

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
                            .FirstOrDefault(r => r.RoleName == "Admin");
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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                return BadRequest("Email không hợp lệ.");
            }
            // Kiểm tra sự tồn tại của tài khoản với email
            var account = await _accountServices.GetAccountByEmail(email);
            if (account == null)
            {
                return NotFound("Email chưa được đăng ký.");
            }
            // Tạo OTP
            string otp = new Random().Next(100000, 1000000).ToString();
            // Gửi OTP qua email
            bool sent = MailHelper.SendOtpEmail(email, otp);
            if (!sent)
            {
                return StatusCode(500, "Không thể gửi email OTP.");
            }
            // Lưu OTP vào hệ thống (có thể lưu vào database hoặc cache)
            OtpStore.SaveOtp(email, otp);

            return Ok("OTP đã được gửi đến email.");
        }


        [HttpPost("/api/auth/verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
                return BadRequest("Email hoặc mã OTP không hợp lệ.");

            // Giả lập xác minh OTP, ví dụ kiểm tra với cache hoặc DB
            bool isValidOtp = OtpStore.VerifyOtp(request.Email, request.Otp); // Giả định bạn có OtpStore
            if (!isValidOtp)
                return BadRequest(new { success = false, message = "Mã OTP không đúng hoặc đã hết hạn." });

            return Ok(new { success = true, message = "OTP hợp lệ." });
        }

        public class VerifyOtpRequest
        {
            public string Email { get; set; }
            public string Otp { get; set; }
        }
        [HttpPost("/api/auth/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest(new { success = false, message = "Email hoặc mật khẩu không hợp lệ." });

            var account = await _accountServices.GetAccountByEmail(request.Email);
            if (account == null)
                return NotFound(new { success = false, message = "Không tìm thấy tài khoản." });

            account.PassWord = HasBCrypt(request.NewPassword); // Bạn nên hash password ở đây
            var result = await _accountServices.UpdateAsync(account);
            if (result)
                return Ok(new { success = true, message = "Đặt lại mật khẩu thành công." });

            return StatusCode(500, new { success = false, message = "Lỗi hệ thống khi đặt lại mật khẩu." });
        }
        [NonAction]
        private static string HasBCrypt(string pass)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(pass);
        }
        public class ResetPasswordRequest
        {
            public string Email { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
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
        [HttpPost("token")]
        public IActionResult GetToken([FromHeader] string token)
        {
            var statusResponse = new StatusResponse();
            return Ok(statusResponse);
        }
    }
}

