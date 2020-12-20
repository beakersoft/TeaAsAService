using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tea.Web.Models
{
    public class SetupNewRound
    {
        public IList<string> UsersInRound { get; set; }

        public string RoundName { get; set; }

        public string RoundLocation { get; set; }


    }
}
