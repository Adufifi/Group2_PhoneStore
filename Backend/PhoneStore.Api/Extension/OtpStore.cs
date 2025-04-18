using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Api.Extension
{
    public static class OtpStore
    {
        private static ConcurrentDictionary<string, (string Otp, DateTime Expiry)> _otpCache
            = new ConcurrentDictionary<string, (string, DateTime)>();

        public static void SaveOtp(string email, string otp, int expireSeconds = 120)
        {
            var expiry = DateTime.Now.AddSeconds(expireSeconds);
            var key = email.Trim().ToLower();
            _otpCache[key] = (otp, expiry);
        }

        public static bool VerifyOtp(string email, string otp)
        {
            var key = email.Trim().ToLower();
            if (_otpCache.TryGetValue(key, out var data))
            {
                if (data.Otp == otp && DateTime.Now <= data.Expiry)
                {
                    _otpCache.TryRemove(key, out _);
                    return true;
                }
            }
            return false;
        }

    }

}