using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEAM_Server.Model.DB.Auth;
using TEAM_Server.Model.DB.Users;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Model.Notification;
using TEAM_Server.Model.RestRequest.Auth;
using TEAM_Server.Services.Interface;
using TEAM_Server.Utilities.Notification;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TEAM_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ISocketService _socket;
        private INotificationService _Notification;
        private IAuthService _Auth;
        private ICategoryService _category;
        public AuthController(
            ISocketService socket,
            IAuthService Auth, 
            INotificationService _Noti,
            ICategoryService category
            )
        {
            _socket = socket;
            _category = category;
            _Notification = _Noti;
            _Auth = Auth; 
        }

        [HttpPost("updateinfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateInfo([FromBody] User_Personal model)
        {
            var status = await _Auth.UpdatePersonalInfo(model);
            if (status)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("namecheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult NameCheck([FromBody] StringClass model)
        {
            var status = _Auth.NameCheck(model.Content);
            if (status)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("getallusers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetAllUsers()
        {
            var status = _Auth.GetAllUsers();
            if (status != null)
            {
                return Ok(status);
            }
            return BadRequest();
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Register([FromBody] User_Register register)
        {
            User user = new User();
            
            user = await _Auth.Register(register);
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                var result = await _socket.CreateInitialConnections(user.uID);
                return Ok(user);
            }
            
        }


        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Authenticate([FromBody] User_Authenticate authenticate)
        {
            User user = new User();
            user = _Auth.Authenticate(authenticate);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
