using System;
using System.Linq;
using Tea.Core.Domain;

namespace Tea.Core.Impl.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TeaContext context, IPasswordHasher passwordHasher)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())            
                return;   // DB has been seeded

            if (passwordHasher != null)
            {
                context.Users.Add(User.CreateLocalDevUser(passwordHasher));
                context.SaveChanges();
            }
        }
    }
}
