using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tea.Core.Data;

namespace Tea.Web.API
{
    [ApiVersion("1.0")]        
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
        [Route("hadbrew")]
        public async Task<IActionResult> HadBrew()
        {
            //create a new user, set the brew count as 1 and return the user
            var localizationString = HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage.OrderBy(x=>x.Quality ?? 0.1).FirstOrDefault()?.Value.ToString()
                ?? "en-GB";

            var user = await _dataStore.CreateNewUserAsync(localizationString);
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