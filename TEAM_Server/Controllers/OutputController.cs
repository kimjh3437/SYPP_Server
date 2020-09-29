using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEAM_Server.Model.Sample;
using TEAM_Server.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TEAM_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OutputController : ControllerBase
    {
        private IOutputService _outputService; 

        public OutputController(IOutputService output)
        {
            _outputService = output;

        }
       

        // GET api/<OutputController>/5
        [HttpPost("insert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Boolean>> Insert([FromBody] Output model )
        {
            Output output = new Output();
            output = _outputService.Insert(model);
            if (output != null)
                return Ok(model);
            else
            {
                return BadRequest();
            }
           
        }

    }
}
