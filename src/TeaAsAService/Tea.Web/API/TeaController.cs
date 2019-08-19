using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;

namespace Tea.Web.API
{
    [ApiVersion("1.0")]        
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TeaController : ControllerBase
    {
        private readonly IAmazonDynamoDB _amazonDynamoDb;

        public TeaController(IAmazonDynamoDB amazonDynamoDb)
        {
            _amazonDynamoDb = amazonDynamoDb;
        }

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