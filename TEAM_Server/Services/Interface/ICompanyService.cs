using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Companies;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Model.RestRequest.Company;
using TEAM_Server.Model.RestRequest.Application;
using TEAM_Server.Model.RestRequest.Checklist;
using TEAM_Server.Model.RestRequest.Contact;
using TEAM_Server.Model.RestRequest.Event;
using TEAM_Server.Model.RestRequest.FollowUp;
using TEAM_Server.Model.RestRequest.Note;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.DB.Checklists;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Events;
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.General.PrimitiveType;

namespace TEAM_Server.Services.Service
{
    public interface ICompanyService
    {
        Company CreateCompany(Company application);
        List<Company> GetCompanies(List_Model list);
        Boolean AddFavoriteCompany(Company_Favorite_Change model);
        Event AddEvent(Event_Request model);
        bool EventSave(Event_Request model);
        bool EventDelete(Event_Request model);
        Note AddNote(Note_Request model);
        bool NoteSave(Note_Request model);
        bool NoteDelete(Note_Request model);
        Contact AddContact(Contact_Request model);
        bool ContactSave(Contact_Request model);
        bool ContactDelete(Contact_Request model);
        FollowUp AddFollowUp(FollowUp_Request model);
        bool FollowUpSave(FollowUp_Request model);
        bool FollowUpDelete(FollowUp_Request model);
        Checklist AddChecklist(Checklist_Request model);
        bool ChecklistSave(Checklist_Request model);
        bool ChecklistDelete(Checklist_Request model);
    }
}
