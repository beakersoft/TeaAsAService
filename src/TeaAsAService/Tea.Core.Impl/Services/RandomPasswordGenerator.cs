using System;

namespace Tea.Core.Impl.Services
{
    public static class RandomPasswordGenerator
    {
        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "123456789";
        const string SPECIALS = @"!@£$%^&*()#€";

        public static string GeneratePassword(int passwordSize)
        {
            char[] _password = new char[passwordSize];
            var charSet = string.Empty; 
            Random _random = new Random();
            
            charSet += LOWER_CASE;
            charSet += UPPER_CAES;
            charSet += NUMBERS;
            charSet += SPECIALS;

            int counter;
            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }

    }
}
