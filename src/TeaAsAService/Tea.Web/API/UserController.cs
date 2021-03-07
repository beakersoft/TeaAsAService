using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tea.Core.Data;
using Tea.Web.Models;
using Tea.Core;
using Tea.Core.Extensions;
using System;
using Tea.Core.Domain;

namespace Tea.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        private readonly IDataStore _dataStore;
        private readonly IPasswordHasher _passwordHasher;

        public UserController(IDataStore dataStore, IPasswordHasher passwordHasher)
        {
            _dataStore = dataStore;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid Create User Request", GetModelStateMessages());

            if (string.IsNullOrEmpty(model.LocalizedString))
            {
                 model.LocalizedString = HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage.OrderByDescending(x=>x.Quality ?? 0.1).FirstOrDefault()?.Value.ToString()
                ?? "en-GB";
            }         
            
            var user = Core.Domain.User.CreateNewUser(model.LocalizedString, model.Firstname, model.Surname);

            if (!user.SetPassword(model.Password, _passwordHasher))
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid Create User Request", "Password does not meet complexity requirements");

            if (!user.SetEmail(model.EmailAddress))
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid Create User Request", "Email Address is not valid");

            user = await _dataStore.CreateAsync(user);

            return Ok(user);
        }

        [HttpPut]
        [Route("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel model)
        {
            if (!ModelState.IsValid)
                return ReturnError(StatusCodes.Status400BadRequest, "Invalid Update User Request", GetModelStateMessages());

            var user = await _dataStore.GetUserBySimpleIdAsync(model.SimpleId);
            var userId = Guid.Parse(User.GetUserId());

            if (userId != user.Id)
                return ReturnError(StatusCodes.Status403Forbidden, "Invalid Update User Request", $"Logged in user cant update {userId.ToString()}");

            if (user == null)
               return ReturnError(StatusCodes.Status404NotFound, "Invalid Update User Request", $"User {model.SimpleId} not found");

            model.UpdateUserFromModel(user, _passwordHasher);
            await _dataStore.UpdateAsync(user);

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(User.GetUserId());
            var user = await _dataStore.GetAsync<User>(userId);

            if (user == null)
                return ReturnError(StatusCodes.Status404NotFound, "Invalid Update User Request", $"User {userId.ToString()} not found");

            return Ok(user);
        }
    }
}
