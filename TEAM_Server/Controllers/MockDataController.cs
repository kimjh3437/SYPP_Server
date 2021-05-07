using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.DB.Companies;
using TEAM_Server.Model.DB.Templates;
using TEAM_Server.Model.DB.Users;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MockDataController : ControllerBase
    {
        IMockDataService _Mock;
        public MockDataController(IMockDataService Mock)
        {
            _Mock = Mock;
        }
        //[HttpGet("create")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<Boolean> Create()
        //{
        //    await _Mock.CreateDummyUser();
        //    var status = await _Mock.CreateDummyModels();
        //    await _Mock.LoadApplication();
        //    return status;
        //}
        //[HttpGet("getuser")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<User> GetUser()
        //{
        //    var status = await _Mock.GetUser();
        //    return status;
        //}

        //[HttpGet("getapplications")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<List<Application>> GetApplications()
        //{
        //    var status = await _Mock.GetApplications();
        //    return status;
        //}
        //[HttpGet("getcompanies")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<List<Company>> GetCompanies()
        //{
        //    var status = await _Mock.GetCompanies();
        //    return status;
        //}
        //[HttpGet("gettemplates")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<List<Template>> GetTemplates()
        //{
        //    var status = await _Mock.GetTemplates();
        //    return status;
        //}
    }
}
