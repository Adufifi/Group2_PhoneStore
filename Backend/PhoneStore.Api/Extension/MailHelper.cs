using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PhoneStore.Api.Extension
{
    public static class MailHelper
    {
        public static bool SendOtpEmail(string toEmail, string otp)
        {
            try
            {
                var fromAddress = new MailAddress("anhnthe172469@fpt.edu.vn", "Mail");
                var toAddress = new MailAddress(toEmail);
                const string fromPass = "mrju qfuy crvb geao";
                const string subject = "Mã OTP đặt lại mật khẩu";
                string body = $"Mã OTP của bạn là: {otp}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPass),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi gửi email: " + ex.Message);
                return false;
            }
        }
    }

}