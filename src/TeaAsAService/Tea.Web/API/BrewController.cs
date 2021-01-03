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
    public class BrewController : ControllerBase
    {
        private IDataStore _dataStore; 

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
            
            var user = await _dataStore.CreateNewUserAsync(localizationString, RandomPasswordGenerator.GeneratePassword(16));
            return Ok(user);
        }

        [HttpPost]        
        [Route("hadbrew")]
        public async Task<IActionResult> HadBrew([FromBody] UserHadBrew model)
        {
            if(string.IsNullOrEmpty(model.UserId))           
                return NotFound("Please pass a user id");
            
            var user = await _dataStore.UpdateBrewCount(model.UserId);

            if (user == null)            
                return NotFound($"Nothing found for user id {model.UserId}");

            return Ok(user);
        }
    }
}