using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Tea.Core;
using System.Xml.Linq;

namespace Tea.Test
{
    public class PasswordTests
    {
        [Fact]
        public void PasswordValidates()
        {
            var passwordTooShort = "Pass1!";
            var passwordNoUppercase = "password1!";
            var passwordNoLowercase = "PASSWORD1!";
            var passwordNoDigits = "Password!";
            var passwordNoSpecialCharacter = "Password1";
            var passwordValid = "Password1!";

            Assert.False(passwordTooShort.ValidatePassword());
            Assert.False(passwordNoUppercase.ValidatePassword());
            Assert.False(passwordNoLowercase.ValidatePassword());
            Assert.False(passwordNoDigits.ValidatePassword());
            Assert.False(passwordNoSpecialCharacter.ValidatePassword());
            Assert.True(passwordValid.ValidatePassword());

        }
    }
}
