using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid round creation request", GetModelStateMessages());
  
            var round = await model.CreateRoundFromModel(_dataStore);

            if (!model.IsRoundValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid round creation request", model.AllValidationMessages);
            
            await _dataStore.CreateAsync(round);

            return Accepted(RoundSummaryModel.FromRound(round));
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid round edit request", GetModelStateMessages());

            if (!model.Id.HasValue)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid round edit request", "Please send a valid round id to edit a round");

            var round = await _dataStore.GetAsync<Round>(model.Id.Value);

            if (round == null)
                return ReturnError(StatusCodes.Status404NotFound, "Invalid round edit request", $"Round {model.Id} not found");

            round = await model.UpdateRound(round, _dataStore);
            
            if (!model.IsRoundValid || round == null)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid round edit request", model.AllValidationMessages);

            round = await _dataStore.UpdateAsync(round);

            return Accepted(RoundSummaryModel.FromRound(round));
        }

        [HttpPost]
        [Route("hadround")]
        public async Task<IActionResult> HadRound([FromBody] HadRoundModel model)
        {
            if (!ModelState.IsValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid had round request", GetModelStateMessages());
          
            var round = await _dataStore.GetAsync<Round>(model.Id);

            if (round == null)
                return ReturnError(StatusCodes.Status404NotFound, "Invalid had round request", $"Round {model.Id} not found");
            
            await _roundService.UpdateExistingRoundAsync(round, _dataStore,model.UserGettingRound,model.RoundNotes);

            return Accepted(RoundSummaryModel.FromRound(round));
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var round = await _dataStore.GetAsync<Round>(id);
            return Ok(RoundSummaryModel.FromRound(round));
        }
    }
}
