using System;
using System.Collections.Generic;

namespace Tea.Core.Domain
{
    public interface IBaseDomain
    {
        DateTime CreatedUtc { get; set; }
        Guid Id { get; set; }
        IList<UpdateEvent> UpdateEvents { get; set; }
    }
}