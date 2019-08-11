using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tea.Web.API
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TeaController : ControllerBase
    {
        [HttpPost]
        public IActionResult Add()
        {
            //create a new id
            return Ok();
        }

        [HttpPost]
        public IActionResult Add(string id)
        {

            return Ok();
        }


        [HttpGet]
        [Route("get/{id}")]
        public IActionResult Get(string id)
        {

            return Ok();
        }
    }
}