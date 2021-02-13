using System;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public abstract class BaseDomain : IBaseDomain
    {
        [Key]
        public Guid Id { get; set; }

        public string UpdateEvents { get; set; }

        public DateTime CreatedUtc { get; set; }
    }
}
