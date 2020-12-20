using System;
using System.Linq;
using Tea.Core.Domain;

namespace Tea.Core.Impl.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TeaContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())            
                return;   // DB has been seeded
            
            context.Users.Add(User.CreateLocalDevUser());
            context.SaveChanges();
        }
    }
}
