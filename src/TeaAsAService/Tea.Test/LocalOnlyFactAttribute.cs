using System;
using Xunit;

namespace Tea.Test
{
    public class LocalOnlyFactAttribute : FactAttribute
    {
        public LocalOnlyFactAttribute()
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_BUILD")))
                Skip = "Ignored on CI Agent";
        }
    }

}
