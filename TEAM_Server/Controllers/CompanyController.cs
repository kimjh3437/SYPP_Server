using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Checklists;
using TEAM_Server.Model.DB.Companies;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Events;
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.DTO.Company;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private IAuthService _auth;
        private ICompanyService _company;
        public CompanyController(
            IAuthService auth,
            ICompanyService company)
        {
            _auth = auth;
            _company = company;
        }

        //___________________________________________________________________________________
        //
        // Application Related - Below
        //___________________________________________________________________________________
        [HttpPost("{uID}/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Company>> CreateCompany(string uID, [FromBody] Company_Detail param)
        {
            var company = await _company.CreateCompany(param);
            var result = await _auth.UpdateUserCompanyID(uID, company.Detail.companyID, false);
            if(result && company != null)
            {
                return Ok(company);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("{uID}/UpdateIsFavorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateIsFavorite([FromBody] Company_IsFavorite_Update_DTO param)
        {
            var result = await _company.ChangeCompanyIsFavorite(param);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        // POST applications/create
        [HttpGet("{uID}/GetCompanies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Company>>> GetCompanies(string uID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                List<string> ids = await _auth.GetcompanyIDs(uID);
                List<Company> companies = new List<Company>();
                if (ids != null)
                {
                    companies = await _company.GetCompanies(ids);
                    if (companies != null)
                        return Ok(companies);
                    else
                        return BadRequest();
                }
                else
                    return BadRequest();
            }
            return BadRequest();
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
                var output = await _company.CreateNote(note);
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
                var output = await _company.UpdateNote(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{companyID}/{noteID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Note>> GetNote(string uID, string companyID, string noteID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _company.GetNotes(uID, companyID, noteID);
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
                var output = await _company.CreateEvent(param);
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
                var output = await _company.UpdateEvent(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost("{uID}/{companyID}/{eventID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> GetEvent(string uID, string companyID, string eventID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _company.GetEvent(uID, companyID, eventID);
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
                var output = await _company.CreateContact(param);
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
                var output = await _company.UpdateContact(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{companyID}/{contactID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> GetContact(string uID, string companyID, string contactID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _company.GetContacts(uID, companyID, contactID);
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
                var output = await _company.CreateFollowUp(param);
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
                var output = await _company.UpdateConvoHistory(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{companyID}/{followUpID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> GetFollowUp(string uID, string companyID, string followUpID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _company.GetFollowUp(uID, companyID, followUpID);
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
                var output = await _company.CreateChecklist(param);
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
                var output = await _company.UpdateChecklist(param);
                if (output != null)
                {
                    return Ok(output);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet("{uID}/{companyID}/{checklistID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> GetChecklist(string uID, string companyID, string checklistID)
        {
            if (!String.IsNullOrEmpty(uID))
            {
                var output = await _company.GetChecklist(uID, companyID, checklistID);
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
