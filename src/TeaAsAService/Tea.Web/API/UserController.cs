using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tea.Core.Data;
using Tea.Core;
using Tea.Web.Models;

namespace Tea.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IDataStore _dataStore;

        public UserController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }


            if(string.IsNullOrEmpty(model.LocalizedString))
            {
                 model.LocalizedString = HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage.OrderByDescending(x=>x.Quality ?? 0.1).FirstOrDefault()?.Value.ToString()
                ?? "en-GB";
            }         

            if(!model.Password.ValidatePassword())
            {
                return BadRequest("Password is not valid. Please enter valid password.");
            }
            
            var user = await _dataStore.CreateNewUserAsync(model.LocalizedString, model.Password);
            return Ok(user);
        }

        [HttpPost]
        [Route("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var user = await _dataStore.GetUserBySimpleIdAsync(model.SimpleId);

            if (user == null)
                return NotFound($"Nothing found for user id {model.SimpleId}");

            model.UpdateUserFromModel(user);
            await _dataStore.UpdateUser(user);

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound("Please pass a user id");

            var user = await _dataStore.GetUserBySimpleIdAsync(id);

            if (user == null)
                return NotFound($"Nothing found for user id {id}");

            return Ok(user);
        }
    }
}
