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

namespace TEAM_Server.Services.Interface
{
    public interface ICompanyService
    {
        Task<Company> CreateCompany(Company_Detail input);
        Task<List<Company>> GetCompanies(List<String> list);
        Task<bool> ChangeCompanyIsFavorite(Company_IsFavorite_Update_DTO model);

        //___Events Related - Below___
        Task<Event> CreateEvent(Event param);
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
        Task<Event> GetEvent(string uID, string companyID, string eventID);


        //___Notes Related - Below___
        Task<Note> GetNotes(string uID, string companyID, string noteID);


        //___Contacts Related - Below___
        Task<Contact> GetContacts(string uID, string companyID, string contactID);



        //___Follow Ups Related - Below___
        Task<FollowUp> GetFollowUp(string uID, string companyID, string followUpID);



        //___Checklists Related - Below___
        Task<Checklist> GetChecklist(string uID, string companyID, string checklistID);


    }
}
