using System;
using System.Linq;
using Tea.Core.Entity;

namespace Tea.Core.Impl.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TeaContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())            
                return;   // DB has been seeded

            var user = new User()
            {
                Id = Guid.NewGuid(),
                CurrentDayCount = 0,
                EmailAddress = "test@domain.com"
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
