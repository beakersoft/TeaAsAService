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
            var charSet = ""; 
            System.Random _random = new Random();
            var counter = 0;

            charSet += LOWER_CASE;
            charSet += UPPER_CAES;
            charSet += NUMBERS;
            charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }

    }
}
