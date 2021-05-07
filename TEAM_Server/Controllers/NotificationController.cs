using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEAM_Server.Model.DB.Users;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Model.Notification;
using TEAM_Server.Services.Interface;
using TEAM_Server.Utilities.Notification;

namespace TEAM_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationService _Notification;
        public NotificationController(INotificationService _Noti)
        {
            _Notification = _Noti;
        }
        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> UploadInstallation([FromBody] NotificationSubscription model)
        {
            var status = _Notification.UploadInstallation(model);
            if (status)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("getinstallation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<NotificationSubscription> GetInstallation([FromBody] StringClass model)
        {
            var status = _Notification.GetInstallation(model.Content);
            if (status != null)
            {
                return Ok(status);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("installations")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> UpdateInstallation(
        [Required] DeviceInstallation deviceInstallation)
        {
            var success = await _Notification
                .CreateOrUpdateInstallationAsync(deviceInstallation, HttpContext.RequestAborted);
            if (!success)
                return new UnprocessableEntityResult();
            return new OkResult();
        }

        [HttpDelete()]
        [Route("installations/{installationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<ActionResult> DeleteInstallation(
            [Required][FromRoute] string installationId)
        {
            var success = await _Notification
                .DeleteInstallationByIdAsync(installationId, CancellationToken.None);
            if (!success)
                return new UnprocessableEntityResult();
            return new OkResult();
        }

        [HttpPost]
        [Route("requests")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> RequestPush(
            [Required] NotificationRequest notificationRequest)
        {
            if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Contents)))
                return new BadRequestResult();
            var success = await _Notification
                .RequestNotificationAsync(notificationRequest);
            if (!success)
                return new UnprocessableEntityResult();
            return new OkResult();
        }
    }
}
