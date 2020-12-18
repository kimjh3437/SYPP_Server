using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Configurations;
using TEAM_Server.Model.DB.Checklists;
using TEAM_Server.Model.DB.Companies;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Events;
using TEAM_Server.Model.DB.File;
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.DB.Users;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Model.RestRequest.Checklist;
using TEAM_Server.Model.RestRequest.Company;
using TEAM_Server.Model.RestRequest.Contact;
using TEAM_Server.Model.RestRequest.Event;
using TEAM_Server.Model.RestRequest.FollowUp;
using TEAM_Server.Model.RestRequest.Note;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Services.Service
{
    public class CompanyService : ICompanyService
    {
        private IMongoCollection<Company> _Companies;
        private IMongoCollection<User> _Users;
        private IOptions<MongoDBSettings> _settings;
        private INotificationService _Notification;
        public CompanyService(
            INotificationService Noti,
            IOptions<MongoDBSettings> settings)
        {
            _Notification = Noti;
            _settings = settings;
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Companies = database.GetCollection<Company>(settings.Value.Companies);
            _Users = database.GetCollection<User>(settings.Value.Users);
        }
        public Company CreateCompany(Company model)
        {
            string companyID = Guid.NewGuid().ToString();
            if(model != null)
            {             
                model.companyID = companyID;
            }
            if (model.Detail == null)
            {
                model.Detail = new Company_Detail();
                model.Detail.IsFavorite = false;
                model.companyID = companyID;
            }
            else
            {
                model.Detail.companyID = companyID;
            }
            if(model.Events == null)
            {
                model.Events = new List<Event>();
            }
            if (model.Notes == null)
            {
                model.Notes = new List<Note>();
            }
            if (model.Contacts == null)
            {
                model.Contacts = new List<Contact>();
            }
            if (model.FollowUps == null)
            {
                model.FollowUps = new List<FollowUp>();
            }
            if (model.Checklists == null)
            {
                model.Checklists = new List<Checklist>();
            }
            try
            {
                _Companies.InsertOne(model);
                return model; 
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        public List<Company> GetCompanies(List_Model list)
        {
            List<Company> companies = new List<Company>();
            companies = _Companies.Find<Company>(x => list.list.Contains(x.companyID)).ToList();
            if (companies != null)
                return companies;
            return null;
        }

        public Boolean AddFavoriteCompany(Company_Favorite_Change model)
        {
            if (model != null)
            {
                var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.companyID);
                var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Company>.Filter.And(filter_1, filter_2);
                var fix = Builders<Company>.Update.Set(x => x.Detail.IsFavorite, model.Status);
                var update = _Companies.UpdateOne(filter, fix);
                if (update.IsAcknowledged)
                {
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        public Event AddEvent(Event_Request model)
        {
            if (model.Event != null)
            {
                Event obj = model.Event;
                obj.eventID = Guid.NewGuid().ToString();
                if (obj.Detail == null)
                {
                    obj.Detail = new Event_Detail();
                }
                if (obj.Contents == null)
                {
                    obj.Contents = new List<Contents_Sub>();
                }
                var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
                var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Company>.Filter.And(filter_1, filter_2);
                var add = Builders<Company>.Update.AddToSet(x => x.Events, obj);

                var result = _Companies.UpdateOne(filter, add);
                if (result.IsAcknowledged)
                {
                    return obj;
                }
                return null;
            }
            return null;
        }
        public bool EventSave(Event_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Set(x => x.Events.Where(x => x.eventID == model.Event.eventID).FirstOrDefault(), model.Event);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
        public bool EventDelete(Event_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Pull(x => x.Events, model.Event);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }

        public Note AddNote(Note_Request model)
        {
            if (model.Note != null)
            {
                Note obj = model.Note;
                obj.noteID = Guid.NewGuid().ToString();
                if (obj.Detail == null)
                {
                    obj.Detail = new Note_Detail();
                }
                if (obj.Contents == null)
                {
                    obj.Contents = new List<Contents_Sub>();
                }
                var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
                var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Company>.Filter.And(filter_1, filter_2);
                var add = Builders<Company>.Update.AddToSet(x => x.Notes, obj);

                var result = _Companies.UpdateOne(filter, add);
                if (result.IsAcknowledged)
                {
                    return obj;
                }
                return null;
            }
            return null;
        }
        public bool NoteSave(Note_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Set(x => x.Notes.Where(x => x.noteID == model.Note.noteID).FirstOrDefault(), model.Note);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
        public bool NoteDelete(Note_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Pull(x => x.Notes, model.Note);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }


        public Contact AddContact(Contact_Request model)
        {
            if (model.Contact != null)
            {
                Contact obj = model.Contact;
                obj.contactID = Guid.NewGuid().ToString();
                if (obj.PersonalDetail == null)
                {
                    obj.PersonalDetail = new Contact_Detail();
                }
                if (obj.Email == null)
                {
                    obj.Email = new Contact_Email();
                }
                if (obj.Phone == null)
                {
                    obj.Phone = new Contact_Phone();
                }
                if (obj.Convo == null)
                {
                    obj.Convo = new List<Contents_Sub>();
                }
                var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
                var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Company>.Filter.And(filter_1, filter_2);
                var add = Builders<Company>.Update.AddToSet(x => x.Contacts, obj);

                var result = _Companies.UpdateOne(filter, add);
                if (result.IsAcknowledged)
                {
                    return obj;
                }
                return null;
            }
            return null;
        }
        public bool ContactSave(Contact_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Set(x => x.Contacts.Where(x => x.contactID == model.Contact.contactID).FirstOrDefault(), model.Contact);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
        public bool ContactDelete(Contact_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Pull(x => x.Contacts, model.Contact);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }

        public FollowUp AddFollowUp(FollowUp_Request model)
        {
            if (model.FollowUp != null)
            {
                FollowUp obj = model.FollowUp;
                obj.followUpID = Guid.NewGuid().ToString();
                if (obj.Personnel == null)
                {
                    obj.Personnel = new Contact_Detail();
                }
                if (obj.Description == null)
                {
                    obj.Description = new List<Contents_Sub>();
                }
                var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
                var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Company>.Filter.And(filter_1, filter_2);
                var add = Builders<Company>.Update.AddToSet(x => x.FollowUps, obj);

                var result = _Companies.UpdateOne(filter, add);
                if (result.IsAcknowledged)
                {
                    return obj;
                }
                return null;
            }
            return null;
        }
        public bool FollowUpSave(FollowUp_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Set(x => x.FollowUps.Where(x => x.followUpID == model.FollowUp.followUpID).FirstOrDefault(), model.FollowUp);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }

        public bool FollowUpDelete(FollowUp_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Pull(x => x.FollowUps, model.FollowUp);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }


        public Checklist AddChecklist(Checklist_Request model)
        {
            if (model.Checklist != null)
            {
                Checklist obj = model.Checklist;
                obj.checklistID = Guid.NewGuid().ToString();
                if (obj.Files == null)
                {
                    obj.Files = new List<File>();
                }

                var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
                var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Company>.Filter.And(filter_1, filter_2);
                var add = Builders<Company>.Update.AddToSet(x => x.Checklists, obj);

                var result = _Companies.UpdateOne(filter, add);
                if (result.IsAcknowledged)
                {
                    return obj;
                }
                return null;
            }
            return null;
        }
        public bool ChecklistSave(Checklist_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Set(x => x.Checklists.Where(x => x.checklistID == model.Checklist.checklistID).FirstOrDefault(), model.Checklist);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }

        public bool ChecklistDelete(Checklist_Request model)
        {
            var filter_1 = Builders<Company>.Filter.Eq(x => x.companyID, model.correspondenceID);
            var filter_2 = Builders<Company>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Company>.Filter.And(filter_1, filter_2);
            var update = Builders<Company>.Update.Pull(x => x.Checklists, model.Checklist);

            var result = _Companies.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
    }
}
