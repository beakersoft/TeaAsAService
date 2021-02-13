using System;

namespace Tea.Core.Domain
{
    public interface IBaseDomain
    {
        DateTime CreatedUtc { get; set; }
        Guid Id { get; set; }
        string UpdateEvents { get;set; }
    }
}