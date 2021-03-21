using System;
using System.Collections.Generic;

namespace Tea.Core.Domain
{
    public class Drink : BaseDomain
    {
        public Drink()
        {
            CreatedUtc = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Emoji { get; set; }

        public static List<Drink> DefaultDrinks()
        {
            return new List<Drink> 
            {
                new Drink { Name = "Tea", Description = "A Cup of tea, the classic", Emoji = "☕" },
                new Drink { Name = "Coffee", Description = "A Cup of Coffee,", Emoji = "☕" },
                new Drink { Name = "Lager", Description = "A pint of lager,", Emoji = "🍺" }
            };
        }
    }
}
