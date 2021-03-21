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

            if (!context.Users.Any()) //LPN only do this in test and dev
            {
                if (passwordHasher != null)
                {
                    context.Users.Add(User.CreateLocalDevUser(passwordHasher));
                    context.SaveChanges();
                }
            }

            //create the default drink set
            if (!context.Drink.Any())
            {
                context.Drink.AddRange(Drink.DefaultDrinks());
                context.SaveChanges();
            }
        }
    }
}
