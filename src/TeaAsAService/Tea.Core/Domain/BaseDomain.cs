using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public abstract class BaseDomain : IBaseDomain
    {
        public BaseDomain()
        {
            UpdateEvents = new List<UpdateEvent>();
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedUtc { get; set; }

        public IList<UpdateEvent> UpdateEvents { get; set; }
    }
}
