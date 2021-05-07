using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.DB.Checklists;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Events;
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.DTO.Application;
using TEAM_Server.Model.DTO.Event;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Model.RestRequest.Application;
using TEAM_Server.Model.RestRequest.Checklist;
using TEAM_Server.Model.RestRequest.Contact;
using TEAM_Server.Model.RestRequest.Event;
using TEAM_Server.Model.RestRequest.FollowUp;
using TEAM_Server.Model.RestRequest.Note;

namespace TEAM_Server.Services.Interface
{
    public interface IApplicationsService
    {
        Task<Application> CreateApplication(Application_Create_DTO application);
        Task<MidTask> CreateApplicationMidTask(MidTask param);
        Task<List<Application>> GetApplications(List<String> list);
        Task<bool> ChangeApplicationIsFavorite(Application_IsFavorite_Update_DTO model);


        //___Events Related - Below___
        Task<Event> CreateEvent(Event param);
        Task<bool> UpdateEventContents(Event_Contents_Update_DTO param);
        Task<Event> UpdateEvent(Event param);


        //___Notes Related - Below___
        Task<Note> CreateNote(Note param);
        Task<Note> UpdateNote(Note param);


        //___Contacts Related - Below___
        Task<Contact> CreateContact(Contact param);
        Task<Contact> UpdateContact(Contact param);


        //___Follow Ups Related - Below___
        Task<FollowUp> CreateFollowUp(FollowUp param);
        Task<FollowUp> UpdateConvoHistory(FollowUp param);


        //___Checklists Related - Below___
        Task<Checklist> CreateChecklist(Checklist param);
        Task<Checklist> UpdateChecklist(Checklist param);


        //___________________________________________________________________________________
        //
        // Update Method Type Handlers - Below
        //___________________________________________________________________________________
        //___Events Related - Below___
        Task<bool> UpdateEventDetail(Event_Detail param);
        Task<Event> GetEvent(string uID, string applicationID, string eventID);

        //___Notes Related - Below___
        Task<Note> GetNote(string uID, string applicationID, string noteID);

        //___Contacts Related - Below___
        Task<Contact> GetContact(string uID, string applicationID, string contactID);

        //___Follow Ups Related - Below___
        Task<FollowUp> GetFollowUp(string uID, string applicationID, string followUpID);

        //___Checklists Related - Below___
        Task<Checklist> GetChecklist(string uID, string applicationID, string checklistID);

    }
}
