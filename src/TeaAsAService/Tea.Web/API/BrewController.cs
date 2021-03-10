using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tea.Core;
using Tea.Core.Data;
using Tea.Core.Impl.Services;
using Tea.Web.Models;
using Tea.Core.Extensions;
using Tea.Core.Domain;

namespace Tea.Web.API
{
    /// <summary>
    /// Manage brews for a person
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrewController : BaseController
    {
        private readonly IDataStore _dataStore;
        private readonly IPasswordHasher _passwordHasher;

        public BrewController(IDataStore dataStore, IPasswordHasher passwordHasher)
        {
            _dataStore = dataStore;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// New person has had a brew. Creates a new users with a random username and password
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Succesful NewPersonHadBrew Request</response>
        /// <response code="400">Invalid NewPersonHadBrew Request</response>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [AllowAnonymous]
        [Route("newpersonhadbrew")]
        public async Task<IActionResult> NewPersonHadBrew()
        {
            var localizationString = HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage.OrderByDescending(x => x.Quality ?? 0.1).FirstOrDefault()?.Value.ToString()
                ?? "en-GB";

            var user = Core.Domain.User.CreateNewUser(localizationString);
            var password = RandomPasswordGenerator.GeneratePassword();

            if (!user.SetPassword(password, _passwordHasher))
                return BadRequest("Could not set password");

            user = await _dataStore.CreateAsync(user);

            return Ok(
            new
            {
                user.Id,
                user.SimpleId,
                password
            });
        }

        /// <summary>
        /// Registers that person has had a brew.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Succesful HadBrew Request</response>
        /// <response code="403">Forbidden HadBrew Request</response>
        /// <response code="404">Invalid HadBrew Request</response>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("hadbrew")]
        public async Task<IActionResult> HadBrew([FromBody] UserHadBrewModel model)
        {
            var user = await _dataStore.GetAsync<User>(model.Id);

            if (user == null)
                return ReturnError(StatusCodes.Status404NotFound, "Invalid HadBrew Request", $"User {model.Id} not found");

            if (model.Id.ToString() != User.GetUserId())
                return ReturnError(StatusCodes.Status403Forbidden, "Invalid HadBrew Request", "Logged in user cant update that round");

            var historyEntry = user.UpdateBrewCount();
            if (historyEntry != null)
                await _dataStore.CreateAsync(historyEntry);

            user = await _dataStore.UpdateAsync(user);

            return Ok(user);
        }
    }
}