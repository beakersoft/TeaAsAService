using System;
using System.ComponentModel.DataAnnotations;

namespace Tea.Web.Models
{
    public class HadRoundModel
    {
        [Required] public Guid Id { get; set; }
        [Required] public string UserGettingRound { get; set; }
        public string RoundNotes { get; set; }
    }
}
