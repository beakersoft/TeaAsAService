using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Impl.Services;
using Tea.Web.Models;

namespace Tea.Web.API
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrewController : BaseController
    {
        private readonly IDataStore _dataStore; 

        public BrewController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("newpersonhadbrew")]
        public async Task<IActionResult> NewPersonHadBrew()
        {
            var localizationString = HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage.OrderByDescending(x=>x.Quality ?? 0.1).FirstOrDefault()?.Value.ToString()
                ?? "en-GB";
            
            var user = Core.Domain.User.CreateNewUser(localizationString, RandomPasswordGenerator.GeneratePassword(16));
            user = await _dataStore.CreateAsync(user);

            return Ok(user);
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