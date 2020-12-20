using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tea.Core.Domain;

namespace Tea.Web.Models
{
    public class RoundModel
    {
        public RoundModel()
        {
            UsersInRound = new List<string>();
        }

        public Guid Id { get; set; }

        public IList<string> UsersInRound { get; set; }

        [Required]
        public string RoundDescription { get; set; }

        [Required]
        public string RoundLocationName { get; set; }

        public Round CreateRoundFromModel()
        {
            //we need to create a round link table as well
            //or work out how to create the link in code like user/history

            return new Round
            {
                Id = Guid.NewGuid(),
                CreatedUtc = DateTime.UtcNow,
                RoundDescription = RoundDescription,
            };
        }
    }
}
