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
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private IDataStore _dataStore;

        public RoundController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }



    }
}
