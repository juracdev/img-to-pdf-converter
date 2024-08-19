using System.Text;

namespace ImgToPdfConverter.Utils;

public static class StringUtils
{
    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var random = new Random();
        var stringBuilder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            char randomChar = chars[random.Next(chars.Length)];
            stringBuilder.Append(randomChar);
        }

        return stringBuilder.ToString();
    }
}
