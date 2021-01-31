using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tea.Core.Data;
using Tea.Core.Domain;
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
        [Route("new")]
        public async Task<IActionResult> New([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            if (!await model.ValidateRound(_dataStore))
                return BadRequest();                    

            var round = await _dataStore.CreateAsync(model.CreateRoundFromModel());

            return Ok(RoundModelSummary.FromRound(round));
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            if (!model.Id.HasValue)
                return BadRequest("Please send a valid round id to edit a round");

            var round = await _dataStore.GetAsync<Round>(model.Id.Value);

            if (round == null)
                return BadRequest($"No round found for round id {model.Id.ToString()}");

            //if we have a round then try and update it


            return Ok();
        }
    }
}
