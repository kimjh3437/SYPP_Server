using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Configurations;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.DB.Category;
using TEAM_Server.Model.DB.Checklists;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Events;
using TEAM_Server.Model.DB.File;
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.General.PrimitiveType;
using TEAM_Server.Model.RestRequest.Application;
using TEAM_Server.Model.RestRequest.Checklist;
using TEAM_Server.Model.RestRequest.Contact;
using TEAM_Server.Model.RestRequest.Event;
using TEAM_Server.Model.RestRequest.FollowUp;
using TEAM_Server.Model.RestRequest.Note;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Services.Service
{
    public class ApplicationsService : IApplicationsService
    {
        private IMongoCollection<Application> _Applications;
        private IOptions<MongoDBSettings> _settings; 
        public ApplicationsService(
            IOptions<MongoDBSettings> settings)
        {
            _settings = settings;
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Applications = database.GetCollection<Application>(settings.Value.Applications);
        }
        public Application CreateApplication(Application application)
        {
            string applicationID = Guid.NewGuid().ToString();
            if(application != null)
            {
                application.applicationID = applicationID;
                application.authID = application.uID;
                if(application.Tasks == null)
                {
                    application.Tasks = new List<MidTask>(); 
                }
                if(application.Detail == null)
                {
                    application.Detail = new Application_Detail();
                    application.Detail.IsFavorite = false;
                }
                else
                {
                    application.Detail.applicationID = applicationID;
                }
                if(application.Events == null)
                {
                    application.Events = new List<Event>();
                }
                if (application.Notes == null)
                {
                    application.Notes = new List<Note>();
                }
                if(application.Contacts == null)
                {
                    application.Contacts = new List<Contact>();
                }
                if(application.FollowUps == null)
                {
                    application.FollowUps = new List<FollowUp>();
                }
                if(application.Checklists == null)
                {
                    application.Checklists = new List<Checklist>();
                }
                try
                {
                    _Applications.InsertOne(application);
                    return application;
                }
                catch (Exception e)
                {
                    return null;
                }  
            }
            //TODO 
            return null;
        }
    
        public List<Application> GetApplications(List_Model list)
        {
            List<Application> Apps = new List<Application>();
            Apps = _Applications.Find<Application>(x => list.list.Contains(x.applicationID)).ToList();
            if (Apps != null)
                return Apps;
            return null;
        }

        public Boolean AddFavoriteApplication(Application_Favorite_Change model)
        {
            if(model != null)
            {
                var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID,  model.applicationID);
                var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Application>.Filter.And(filter_1, filter_2);
                var fix = Builders<Application>.Update.Set(x => x.Detail.IsFavorite, model.Status);
                var update = _Applications.UpdateOne(filter, fix);
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
            if(model.Event != null)
            {
                Event obj = model.Event;
                obj.eventID = Guid.NewGuid().ToString();
                if(obj.Detail == null)
                {
                    obj.Detail = new Event_Detail();
                }
                if(obj.Contents == null)
                {
                    obj.Contents = new List<Contents_Sub>();
                }
                var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
                var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Application>.Filter.And(filter_1, filter_2);
                var add = Builders<Application>.Update.AddToSet(x => x.Events, obj);

                var result = _Applications.UpdateOne(filter, add);
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
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Set(x => x.Events.Where(x => x.eventID == model.Event.eventID).FirstOrDefault(), model.Event);

            var result = _Applications.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
        public bool EventDelete(Event_Request model)
        {
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Pull(x => x.Events, model.Event);

            var result = _Applications.UpdateOne(filter, update);
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
                var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
                var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Application>.Filter.And(filter_1, filter_2);
                var add = Builders<Application>.Update.AddToSet(x => x.Notes, obj);

                var result = _Applications.UpdateOne(filter, add);
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
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Set(x => x.Notes.Where(x => x.noteID == model.Note.noteID).FirstOrDefault(), model.Note);

            var result = _Applications.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
        public bool NoteDelete(Note_Request model)
        {
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Pull(x => x.Notes, model.Note);

            var result = _Applications.UpdateOne(filter, update);
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
                var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
                var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Application>.Filter.And(filter_1, filter_2);
                var add = Builders<Application>.Update.AddToSet(x => x.Contacts, obj);

                var result = _Applications.UpdateOne(filter, add);
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
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Set(x => x.Contacts.Where(x => x.contactID == model.Contact.contactID).FirstOrDefault(), model.Contact);

            var result = _Applications.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
        public bool ContactDelete(Contact_Request model)
        {
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Pull(x => x.Contacts, model.Contact);

            var result = _Applications.UpdateOne(filter, update);
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
                var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
                var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Application>.Filter.And(filter_1, filter_2);
                var add = Builders<Application>.Update.AddToSet(x => x.FollowUps, obj);

                var result = _Applications.UpdateOne(filter, add);
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
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Set(x => x.FollowUps.Where(x => x.followUpID == model.FollowUp.followUpID).FirstOrDefault(), model.FollowUp);

            var result = _Applications.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }

        public bool FollowUpDelete(FollowUp_Request model)
        {
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Pull(x => x.FollowUps, model.FollowUp);

            var result = _Applications.UpdateOne(filter, update);
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
          
                var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
                var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
                var filter = Builders<Application>.Filter.And(filter_1, filter_2);
                var add = Builders<Application>.Update.AddToSet(x => x.Checklists, obj);

                var result = _Applications.UpdateOne(filter, add);
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
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Set(x => x.Checklists.Where(x => x.checklistID == model.Checklist.checklistID).FirstOrDefault(), model.Checklist);

            var result = _Applications.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }

        public bool ChecklistDelete(Checklist_Request model)
        {
            var filter_1 = Builders<Application>.Filter.Eq(x => x.applicationID, model.correspondenceID);
            var filter_2 = Builders<Application>.Filter.Eq(x => x.uID, model.uID);
            var filter = Builders<Application>.Filter.And(filter_1, filter_2);
            var update = Builders<Application>.Update.Pull(x => x.Checklists, model.Checklist);

            var result = _Applications.UpdateOne(filter, update);
            if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
    }
}
