using Xunit;
using Tea.Core;

namespace Tea.Test
{
    public class PasswordAndEmailValidationTests
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

        [Theory]
        [InlineData("johndoe", false)]
        [InlineData("johndoe.com", false)]
        [InlineData("johndoe@", false)]
        [InlineData("johndoe@url", true)]
        [InlineData("johndoe@url.com", true)]
        public void EmailValidates(string emailToCheck, bool shouldValidate)
        {
            Assert.Equal(shouldValidate, emailToCheck.ValidateEmail());
        }
    }
}
