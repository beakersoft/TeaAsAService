using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tea.Core.Data;

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

    }
}
