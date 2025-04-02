using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Api.Helper
{
    public static class ImageHelper
    {
        public static string ConvertToBase64(byte[] imageBytes)
        {
            return Convert.ToBase64String(imageBytes);
        }
        public static byte[] ConvertImageToByteArray(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Image path cant be null or empty.", nameof(imagePath));
            }
            if (imagePath.Contains(","))
            {
                var base64String = imagePath.Split(',')[1];
                return Convert.FromBase64String(base64String);
            }
            else
            {
                return System.IO.File.ReadAllBytes(imagePath);
            }
        }
    }
}