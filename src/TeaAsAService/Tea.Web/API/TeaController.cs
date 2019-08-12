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
            //create a new id and insert the row as 1
            return Ok();
        }

        [HttpPost]        
        public IActionResult Add(string id)
        {
            //lookup the id update the count.

            //if we have rolled over midnight copy todays brew into brew summary table
            return Ok();
        }




        [HttpGet]
        [Route("get/{id}")]
        public IActionResult Get(string id)
        {
            //get the current day out as well as the summary brews
            return Ok();
        }
    }
}