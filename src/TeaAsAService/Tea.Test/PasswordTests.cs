using Xunit;
using Tea.Core;

namespace Tea.Test
{
    public class PasswordTests
    {
        [Theory]
        [InlineData("Pass1!", false)]
        [InlineData("password1!", false)]
        [InlineData("PASSWORD1!", false)]
        [InlineData("Password!", false)]
        [InlineData("Password1", false)]
        [InlineData("Password1!", true)]
        [InlineData("IamAC0mpl£xStr1ng", true)]
        public void PasswordValidates(string passwordToCheck, bool shouldValidate)
        {
            Assert.Equal(shouldValidate, passwordToCheck.ValidatePassword());
        }
    }
}
