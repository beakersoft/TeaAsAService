using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tea.Core;
using Tea.Core.Data;
using Tea.Core.Domain;
using Tea.Web.Models;

namespace Tea.Web.API
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : BaseController
    {
        private readonly IDataStore _dataStore;
        private readonly IRoundService _roundService;

        public RoundController(IDataStore dataStore, IRoundService roundService)
        {
            _dataStore = dataStore;
            _roundService = roundService;
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> New([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(GetModelStateMessages());

            var round = await model.CreateRoundFromModel(_dataStore);

            if (!model.IsRoundValid)
                return BadRequest(model.AllValidationMessages);   

            await _dataStore.CreateAsync(round);

            return Accepted(RoundSummaryModel.FromRound(round));
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(GetModelStateMessages());
            
            if (!model.Id.HasValue)
                return BadRequest("Please send a valid round id to edit a round");

            var round = await _dataStore.GetAsync<Round>(model.Id.Value);

            if (round == null)
                return NotFound($"No round found for round id {model.Id}");

            round = await model.UpdateRound(round, _dataStore);
            
            if (!model.IsRoundValid || round == null)
                return BadRequest(model.AllValidationMessages);

            round = await _dataStore.UpdateAsync(round);

            return Accepted(RoundSummaryModel.FromRound(round));
        }

        [HttpPost]
        [Route("hadround")]
        public async Task<IActionResult> HadRound([FromBody] HadRoundModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(GetModelStateMessages());

            var round = await _dataStore.GetAsync<Round>(model.Id);

            if (round == null)
                return NotFound($"No round found for round id {model.Id}");
            
            await _roundService.UpdateExistingRoundAsync(round,model.UserGettingRound,model.RoundNotes);

            return Accepted(RoundSummaryModel.FromRound(round));
        }
    }
}
