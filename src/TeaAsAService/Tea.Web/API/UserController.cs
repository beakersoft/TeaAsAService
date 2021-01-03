using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tea.Core.Data;
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
