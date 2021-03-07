using System;

namespace Tea.Core.Impl.Services
{
    public static class RandomPasswordGenerator
    {
        private const string LowerCase = "abcdefghijklmnopqursuvwxyz";
        private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "123456789";
        private const string Specials = @"!@£$%^&*()#€";

        public static string GeneratePassword(int lowers =2,int uppers = 2,int numbers = 2,int specials = 2)
        {
            var random = new Random();

            string generated = string.Empty;
            generated = AddCharsToString(generated, LowerCase, random, lowers);
            generated = AddCharsToString(generated, UpperCase, random, uppers);
            generated = AddCharsToString(generated, Numbers, random, numbers);
            generated = AddCharsToString(generated, Specials, random, specials);

            return generated;
        }

        private static string AddCharsToString(string destination, string charsToUse, Random random, int charsToInsert)
        {
            for (int i = 1; i <= charsToInsert; i++)
                destination = destination.Insert(
                    random.Next(destination.Length),
                    charsToUse[random.Next(charsToUse.Length)].ToString()
                );

            return destination;
        }
    }
}
