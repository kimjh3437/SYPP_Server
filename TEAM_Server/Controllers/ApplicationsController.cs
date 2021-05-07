using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.DB.Category;
using TEAM_Server.Model.DB.Checklists;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Events;
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.DTO.Application;
using TEAM_Server.Model.DTO.Event;
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
        private IAuthService _auth;
        private ICategoryService _category;
        public ApplicationsController(
            IAuthService auth,
            ICategoryService category,
            IApplicationsService applications)
        {
            _auth = auth;
            _category = category;
            _applications = applications;
        }


        //___________________________________________________________________________________
        //
        // Application Related - Below
        //___________________________________________________________________________________
        [HttpPost("{uID}/Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Application>> CreateApplication(string uID, [FromBody] Application_Create_DTO model) 
        {
            Application application = new Application();
            if (model != null)
            {
                if(model.Detail.Categories != null && model.Detail.Categories.Count != 0)
                {
                    List<Category> Categories = new List<Category>();
                    List<Task> Tasks = new List<Task>();
                    foreach (var item in model.Detail.Categories)
                    {

                        //saves to category database 
                        var category = await _category.AddCategory(item);

                        //updates preferences on user 
                        Tasks.Add(_auth.UpdateUserPreferences(category, uID));
                        Categories.Add(category);
                    }
                    model.Detail.Categories = Categories;
                    await Task.WhenAll(Tasks);
                }
                
                //creates appplication on application db
                application = await _applications.CreateApplication(model);

                //updates applicationIDs on user
                var result = await _auth.UpdateUserApplicationID(uID, application.applicationID, false);
                if (application != null)
                    return Ok(application);
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }
        [HttpPost("{uID}/CreateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MidTask>> CreateApplicationMidTask(string uID, [FromBody] MidTask model)
        {
            if (model != null)
            {
                //creates appplication on application db
                var task = await _applications.CreateApplicationMidTask(model);

                if (task != null)
                    return Ok(task);
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }

        [HttpGet("{uID}/GetApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Application>>> GetApplications(string uID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                List<string> appIDs = await _auth.GetApplicationIDs(uID);
                List<Application> applications = new List<Application>();
                if (appIDs != null)
                {
                    applications = await _applications.GetApplications(appIDs);
                    if (applications != null)
                        return Ok(applications);
                    else
                        return BadRequest();
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost("{uID}/UpdateIsFavorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateIsFavorite([FromBody] Application_IsFavorite_Update_DTO param)
        {
            var result = await _applications.ChangeApplicationIsFavorite(param);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        //___________________________________________________________________________________
        //
        // Application Notes Related - Below
        //___________________________________________________________________________________
        [HttpPost("{uID}/CreateNote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Note>> CreateNote(string uID, [FromBody] Note note)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.CreateNote(note);
                if (output != null)
                {
                    return Ok(output);     
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost("{uID}/UpdateNote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Note>> UpdateNote(string uID, [FromBody] Note param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.UpdateNote(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{applicationID}/{noteID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Note>> GetNote(string uID, string applicationID, string noteID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.GetNote(uID, applicationID, noteID);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }

        //___________________________________________________________________________________
        //
        // Application Events Related - Below
        //___________________________________________________________________________________
        [HttpPost("{uID}/CreateEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> CreateEvent(string uID, [FromBody] Event param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.CreateEvent(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost("{uID}/UpdateEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> UpdateEvent(string uID, [FromBody] Event param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.UpdateEvent(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{applicationID}/{eventID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> GetEvent(string uID, string applicationID, string eventID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.GetEvent(uID, applicationID, eventID);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }

        //___________________________________________________________________________________
        //
        // Application Contacts Related - Below
        //___________________________________________________________________________________
        [HttpPost("{uID}/CreateContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Contact>> CreateContact(string uID, [FromBody] Contact param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.CreateContact(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost("{uID}/UpdateContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Contact>> UpdateContact(string uID, [FromBody] Contact param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.UpdateContact(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{applicationID}/{contactID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Contact>> GetContact(string uID, string applicationID, string contactID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.GetContact(uID, applicationID, contactID);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }

        //___________________________________________________________________________________
        //
        // Application Follow Up Related - Below
        //___________________________________________________________________________________
        [HttpPost("{uID}/CreateFollowUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FollowUp>> CreateFollowUp(string uID, [FromBody] FollowUp param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.CreateFollowUp(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost("{uID}/UpdateFollowUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FollowUp>> UpdateFollowUp(string uID, [FromBody] FollowUp param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.UpdateConvoHistory(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{applicationID}/{followUpID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FollowUp>> GetFollowUp(string uID, string applicationID, string followUpID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.GetFollowUp(uID, applicationID, followUpID);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }

        //___________________________________________________________________________________
        //
        // Application Checklist Related - Below
        //___________________________________________________________________________________
        [HttpPost("{uID}/CreateChecklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Checklist>> CreateChecklist(string uID, [FromBody] Checklist param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.CreateChecklist(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost("{uID}/UpdateChecklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Checklist>> UpdateChecklist(string uID, [FromBody] Checklist param)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.UpdateChecklist(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{applicationID}/{checklistID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Checklist>> GetChecklist(string uID, string applicationID,string checklistID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _applications.GetChecklist(uID, applicationID, checklistID);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
    }
}
