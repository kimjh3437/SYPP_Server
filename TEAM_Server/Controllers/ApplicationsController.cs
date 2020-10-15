using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TEAM_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private IApplicationsService _applications;
        public ApplicationsController(
            IApplicationsService applications)
        {
            _applications = applications;
        }
        // GET: api/<ApplicationsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ApplicationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST applications/create
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Application> CreateApplication([FromBody] Application model) 
        {
            Application application = new Application();
            if (model != null)
            {
                application = _applications.CreateApplication(model);
                if (application != null)
                    return Ok(application);
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }

        // POST applications/create
        [HttpPost("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Application>> GetAllApplications([FromBody] List_Model model)
        {
            List<Application> applications = new List<Application>();
            if (model != null)
            {
                applications = _applications.GetApplications(model);
                if (applications != null)
                    return Ok(applications);
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }
    }
}
