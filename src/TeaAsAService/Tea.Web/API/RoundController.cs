using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tea.Core.Data;
using Tea.Web.Models;

namespace Tea.Web.API
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public RoundController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpPost]
        [Route("SetupNewRound")]
        public async Task<IActionResult> SetupNewRound([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            if (model.UsersInRound.Count() < 2)
                return BadRequest("You need at least 2 people in a round");

            var round = await _dataStore.CreateAsync(model.CreateRoundFromModel());

            return Ok(round);
        }
    }
}
