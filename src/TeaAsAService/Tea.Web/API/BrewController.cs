using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tea.Core;
using Tea.Core.Data;
using Tea.Core.Impl.Services;
using Tea.Web.Models;

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
        [HttpPost]
        [AllowAnonymous]
        [Route("newpersonhadbrew")]
        public async Task<IActionResult> NewPersonHadBrew()
        {
            var localizationString = HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage.OrderByDescending(x=>x.Quality ?? 0.1).FirstOrDefault()?.Value.ToString()
                ?? "en-GB";
            
            var user = Core.Domain.User.CreateNewUser(localizationString);
            var password = RandomPasswordGenerator.GeneratePassword(16);
                        
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

        [HttpPost]        
        [Route("hadbrew")]
        public async Task<IActionResult> HadBrew([FromBody] UserHadBrewModel model)
        {
            if(string.IsNullOrEmpty(model.UserId))           
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid HadBrew Request", "Please pass a user id");

            var user = await _dataStore.GetUserBySimpleIdAsync(model.UserId);

            if (user == null)
                return ReturnError(StatusCodes.Status404NotFound, "Invalid HadBrew Request", $"User {model.UserId} not found");
           
            var historyEntry = user.UpdateBrewCount();
            if (historyEntry != null)
                await _dataStore.CreateAsync(historyEntry);
            
            user = await _dataStore.UpdateAsync(user);

            return Ok(user);
        }
    }
}