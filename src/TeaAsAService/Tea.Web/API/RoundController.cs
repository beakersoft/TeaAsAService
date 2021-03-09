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

        /// <summary>
        /// Creates a new round.
        /// </summary>
        /// <response code="202">Succesful RoundNew Request</response>
        /// <response code="400">Invalid RoundNew Request</response>
        [HttpPost]
        [Route("new")]
        [ProducesResponseType(typeof(RoundSummaryModel), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> New([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid RoundNew Request", GetModelStateMessages());

            var round = await model.CreateRoundFromModel(_dataStore);

            if (!model.IsRoundValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid RoundNew Request", model.AllValidationMessages);

            await _dataStore.CreateAsync(round);

            return Accepted(RoundSummaryModel.FromRound(round));
        }

        /// <summary>
        /// Edits a round.
        /// </summary>
        /// <response code="202">Succesful RoundEdit Request</response>
        /// <response code="400">Invalid RoundEdit Request</response>
        /// <response code="403">Forbidden RoundEdit Request</response>
        /// <response code="404">Invalid RoundEdit Request</response>
        [HttpPut]
        [Route("edit")]
        [ProducesResponseType(typeof(Round), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromBody] RoundModel model)
        {
            if (!ModelState.IsValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid RoundEdit Request", GetModelStateMessages());

            if (!model.Id.HasValue)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid RoundEdit Request", "Please send a valid round id to edit a round");

            var round = await _dataStore.GetAsync<Round>(model.Id.Value);

            if (round == null)
                return ReturnError(StatusCodes.Status404NotFound, "Invalid RoundEdit Request", $"Round {model.Id} not found");

            if (!round.CanUpdateRound(User))
                return ReturnError(StatusCodes.Status403Forbidden, "Forbidden RoundEdit Request", $"You are not allowed to edit this round");

            round = await model.UpdateRound(round, _dataStore);

            if (!model.IsRoundValid || round == null)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid RoundEdit Request", model.AllValidationMessages);

            round = await _dataStore.UpdateAsync(round);

            return Accepted(RoundSummaryModel.FromRound(round));
        }

        /// <summary>
        /// Registers that a user has had a round.
        /// </summary>
        /// <response code="200">Succesful HadRound Request</response>
        /// <response code="400">Invalid HadRound Request</response>
        /// <response code="403">Forbidden HadRound Request</response>
        /// <response code="404">Invalid HadRound Request</response>
        [ProducesResponseType(typeof(Round), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("hadround")]
        public async Task<IActionResult> HadRound([FromBody] HadRoundModel model)
        {
            if (!ModelState.IsValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid HadRound request", GetModelStateMessages());

            var round = await _dataStore.GetAsync<Round>(model.Id);

            if (round == null)
                return ReturnError(StatusCodes.Status404NotFound, "Invalid HadRound request", $"Round {model.Id} not found");

            if (!round.CanUpdateRound(User))
                return ReturnError(StatusCodes.Status403Forbidden, "Invalid HadRound request", $"You are not allowed to edit this round");

            await _roundService.UpdateExistingRoundAsync(round, _dataStore, model.UserGettingRound, model.RoundNotes);

            return Ok(RoundSummaryModel.FromRound(round));
        }

        /// <summary>
        /// Gets a round by Id.
        /// </summary>
        /// <response code="200">Succesful GetRound Request</response>
        /// <response code="403">Forbidden GetRound Request</response>
        [ProducesResponseType(typeof(Round), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var round = await _dataStore.GetAsync<Round>(id);

            if (!round.CanUpdateRound(User))
                return ReturnError(StatusCodes.Status403Forbidden, "Invalid GetRound request", "You are not to access this round");

            return Ok(RoundSummaryModel.FromRound(round));
        }
    }
}
