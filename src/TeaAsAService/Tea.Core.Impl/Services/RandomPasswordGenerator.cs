using System;

namespace Tea.Core.Impl.Services
{
    public static class RandomPasswordGenerator
    {
        const string LowerCase = "abcdefghijklmnopqursuvwxyz";
        const string UpperCaes = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string Numbers = "123456789";
        const string Specials = @"!@£$%^&*()#€";

        public static string GeneratePassword(int passwordSize)
        {
            var password = new char[passwordSize];
            var charSet = string.Empty; 
            var random = new Random();
            
            charSet += LowerCase;
            charSet += UpperCaes;
            charSet += Numbers;
            charSet += Specials;

            int counter;
            for (counter = 0; counter < passwordSize; counter++)
            {
                password[counter] = charSet[random.Next(charSet.Length - 1)];
            }

            return string.Join(null, password);
        }

    }
}
