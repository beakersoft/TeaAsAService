using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tea.Core.Entity;
using Xunit;

namespace Tea.Test.Core.Entity
{
    public class UserFixture
    {        
        [Fact]
        public void NewUserCreates()
        {
            var password = "Password1";
            var locstring = "en-GB";
            var user = User.CreateNewUser(locstring, password);

            Assert.Equal(password, user.Password);
            Assert.Equal(locstring, user.Localization);

        }
    }
}
