using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Impl.Services;

namespace Tea.Web.API
{    
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeaController : ControllerBase
    {
        private IDataStore _dataStore; 

        public TeaController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("hadbrew")]
        public async Task<IActionResult> HadBrew()
        {

            //LPN get the actuak local out so we know when to create a history entry
            var localizationString = HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage.OrderBy(x=>x.Quality ?? 0.1).FirstOrDefault()?.Value.ToString()
                ?? "en-GB";
            
            var user = await _dataStore.CreateNewUserAsync(localizationString, RandomPasswordGenerator.GeneratePassword(16));
            return Ok(user);
        }

        [HttpPost]        
        [Route("hadbrew/{id}")] //LPN THIS NEEDS TO BE FROM BODY
        public async Task<IActionResult> HadBrew(string id)
        {            
            var user = await _dataStore.UpdateBrewCount(id);

            if (user == null)            
                return NotFound();
            
            //if we have rolled over midnight copy todays brew into brew summary table
            return Ok(user);
        }

        [HttpGet]        
        [Route("brews/{id}")]
        public IActionResult Brews(string id)
        {
            

            return Ok();
        }
    }
}