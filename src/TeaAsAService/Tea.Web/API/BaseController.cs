using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Tea.Web.API
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        public string GetModelStateMessages()
        {
            return string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
        }
    }
}
