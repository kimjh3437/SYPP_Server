using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
using TEAM_Server.Model.DTO.Application;
using TEAM_Server.Model.DTO.Event;
using TEAM_Server.Model.DTO.Note;
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
        //___________________________________________________________________________________
        //
        // Template Method - Below
        //___________________________________________________________________________________
        public async Task TemplateMethod(object param1, object param2)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.Write($"<MethodName> : {ex}");
            }
        }

        //___________________________________________________________________________________
        //
        // Create Method Type Handlers - Below
        //___________________________________________________________________________________

        //___Applications Related - Below___
        public async Task<Application> CreateApplication(Application_Create_DTO input)
        {
            try
            {
                var applicationID = Guid.NewGuid().ToString();
                input.Detail.applicationID = applicationID;
                if(input.Detail.Status == null || input.Detail.Status.Count == 0)
                {
                    input.Detail.Status = new List<MidTask>();
                }
                else
                {
                    foreach(var item in input.Detail.Status)
                    {
                        item.applicationID = applicationID;
                    }
                }
                if (input.Detail.Categories == null || input.Detail.Categories.Count == 0)
                {
                    input.Detail.Categories = new List<Category>();
                }
                var application = new Application
                {
                    Detail = input.Detail,
                    Tasks = new List<MidTask>(),
                    Events = new List<Event>(),
                    Notes = new List<Note>(),
                    Contacts = new List<Contact>(),
                    FollowUps = new List<FollowUp>(),
                    Checklists = new List<Checklist>()
                };
                await _Applications.InsertOneAsync(application);
                return application;
            }
            catch (Exception ex)
            {
                Console.Write($"<CreateApplication> : {ex}");
                return null;
            }
        }


        //___Applications Task Related - Below___
        public async Task<MidTask> CreateApplicationMidTask(MidTask param)
        {
            try
            {
                if (String.IsNullOrEmpty(param.applicationID))
                    return null;

                param.midTaskID = Guid.NewGuid().ToString();

                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.applicationID);
                var update = Builders<Application>.Update.AddToSet(x => x.Tasks, param);
                var result = await _Applications.UpdateOneAsync(filter, update);
                return param;
            }
            catch(Exception ex)
            {
                return null;
            } 
        } 


        //___Events Related - Below___
        public async Task<Event> CreateEvent(Event param)
        {
            try
            {
                var eventID = Guid.NewGuid().ToString();
                param.eventID = eventID;
                param.Detail.eventID = eventID;
                foreach (var item in param.Contents)
                {
                    item.belongingID = eventID;
                    foreach (var content in item.Contents)
                    {
                        content.belongingID = eventID;
                    }
                }
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.AddToSet(x => x.Events, param);
                var result = await _Applications.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___Notes Related - Below___
        public async Task<Note> CreateNote(Note param)
        {
            try
            {
                var noteID = Guid.NewGuid().ToString();
                param.noteID = noteID;
                param.Detail.noteID = noteID;
                foreach (var item in param.Contents)
                {
                    item.belongingID = noteID;
                    foreach (var content in item.Contents)
                    {
                        content.belongingID = noteID;
                    }
                }
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.AddToSet(x => x.Notes, param);
                var result = await _Applications.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        //___Contacts Related - Below___
        public async Task<Contact> CreateContact(Contact param)
        {
            try
            {
                var contactID = Guid.NewGuid().ToString();
                param.contactID = contactID;
                param.Detail.contactID = contactID;
                param.Phone.contactID = contactID;
                param.Email.contactID = contactID;
                foreach (var item in param.Convo)
                {
                    item.belongingID = contactID;
                    foreach (var content in item.Contents)
                    {
                        content.belongingID = contactID;
                    }
                }
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.AddToSet(x => x.Contacts, param);
                var result = await _Applications.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___Follow Ups Related - Below___
        public async Task<FollowUp> CreateFollowUp(FollowUp param)
        {
            try
            {
                var followUpID = Guid.NewGuid().ToString();
                param.followUpID = followUpID;
                param.Detail.followUpID = followUpID;
                foreach (var item in param.Description)
                {
                    item.belongingID = followUpID;
                    foreach (var content in item.Contents)
                    {
                        content.belongingID = followUpID;
                    }
                }
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.AddToSet(x => x.FollowUps, param);
                var result = await _Applications.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___Checklists Related - Below___
        public async Task<Checklist> CreateChecklist(Checklist param)
        {
            try
            {
                var checklistID = Guid.NewGuid().ToString();
                param.checklistID = checklistID;
                foreach (var item in param.Options)
                {
                    item.checklistID = checklistID;
                }
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.applicationID);
                var update = Builders<Application>.Update.AddToSet(x => x.Checklists, param);
                var result = await _Applications.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___________________________________________________________________________________
        //
        // Update Method Type Handlers - Below
        //___________________________________________________________________________________

        //___Applications Related - Below___
        public async Task<bool> ChangeApplicationIsFavorite(Application_IsFavorite_Update_DTO model)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, model.applicationID);
                var update = Builders<Application>.Update.Set(x => x.Detail.IsFavorite, model.IsFavorite);
                var result = await _Applications.UpdateOneAsync(filter, update);
                return result.IsAcknowledged;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //___Events Related - Below___

        public async Task<bool> UpdateEventDetail(Event_Detail param)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.Detail.applicationID, param.applicationID);
                var update = Builders<Application>.Update.Set("Events.$[event].Detail", "2cb66");
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("event.event", param.eventID);

                arrayFilters.Add(level1);

                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                return result.IsAcknowledged;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateEventContents(Event_Contents_Update_DTO param)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.Detail.applicationID, param.applicationID);

                //Update Header 
                if (String.IsNullOrEmpty(param.textID))
                {
                    var update = Builders<Application>.Update.Set("Events.$[event].Header", param.Header);
                    var arrayFilters = new List<ArrayFilterDefinition>();
                    ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("event.eventID", param.eventID);
                    arrayFilters.Add(level1);
                    var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                    var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                    return result.IsAcknowledged;
                }

                //Update Contents 
                if (!String.IsNullOrEmpty(param.textID))
                {
                    var update = Builders<Application>.Update.Set("Events.$[event].Contents.$[content].Content", param.Content);
                    var arrayFilters = new List<ArrayFilterDefinition>();
                    ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("event.eventID", param.eventID);
                    ArrayFilterDefinition<BsonDocument> level2 = new BsonDocument("content.textID", param.textID);
                    arrayFilters.Add(level1);
                    arrayFilters.Add(level2);
                    var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                    var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                    return result.IsAcknowledged;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<Event> UpdateEvent(Event param)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.Detail.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.Set("Events.$[event]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("event.eventID", param.eventID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        //___Notes Related - Below___
        public async Task<Note> UpdateNote(Note param)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.Detail.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.Set("Notes.$[note]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("note.noteID", param.noteID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                    return null;
                ////Update Header 
                //if (false)
                //{
                //    var update = Builders<Application>.Update.Set("Notes.$[note].Contents.$[noteContents].", param.Header);
                //    var arrayFilters = new List<ArrayFilterDefinition>();
                //    ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("note.noteID", param.noteID);
                //    ArrayFilterDefinition<BsonDocument> level2 = new BsonDocument("noteContents.noteContentsID", param.noteContentsID);
                //    arrayFilters.Add(level1);
                //    arrayFilters.Add(level2);
                //    var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                //    var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                //    return result.IsAcknowledged;
                //}

                ////Update Contents 
                //if (false)
                //{
                //    var update = Builders<Application>.Update.Set("Notes.$[note].Contents.$[content].Contents", param.Contents);
                //    var arrayFilters = new List<ArrayFilterDefinition>();
                //    ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("note.noteID", param.noteID);
                //    ArrayFilterDefinition<BsonDocument> level2 = new BsonDocument("content.noteContentsID", param.noteContentsID);
                //    arrayFilters.Add(level1);
                //    arrayFilters.Add(level2);
                //    var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                //    var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                //    return result.IsAcknowledged;
                //}
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___Contacts Related - Below___
        public async Task<Contact> UpdateContact(Contact param)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.Set("Contacts.$[contact]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("contact.contactID", param.contactID);
                
                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                    return null;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        //___Follow Ups Related - Below___
        public async Task<FollowUp> UpdateConvoHistory(FollowUp param)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.Detail.applicationID);
                var update = Builders<Application>.Update.Set("FollowUps.$[followup]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("followup.followUpID", param.followUpID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //___Checklists Related - Below___
        public async Task<Checklist> UpdateChecklist(Checklist param)
        {
            try
            {
                var filter = Builders<Application>.Filter.Eq(x => x.applicationID, param.applicationID);
                var update = Builders<Application>.Update.Set("Checklists.$[checklist]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("checklist.checklistID", param.checklistID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Applications.UpdateOneAsync(filter, update, updateOptions);
                if (result.IsAcknowledged)
                {
                    return param;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //___________________________________________________________________________________
        //
        // Get Method Type Handlers - Below
        //___________________________________________________________________________________

        //___Applications Related - Below___
        public async Task<List<Application>> GetApplications(List<String> list)
        {
            try
            {
                List<Application> Apps = new List<Application>();
                Apps = _Applications.Find<Application>(x => list.Contains(x.applicationID)).ToList();
                if (Apps != null)
                    return Apps;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___Events Related - Below___
        public async Task<Event> GetEvent(string uID, string applicationID, string eventID)
        {
            try
            {
                var application = _Applications.Find(x => x.applicationID == applicationID && x.Detail.uID == uID).FirstOrDefault();
                if (application != null)
                {
                    var output = application.Events.Where(x => x.Detail.eventID == eventID).FirstOrDefault();
                    return output;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //___Notes Related - Below___
        public async Task<Note> GetNote(string uID, string applicationID, string noteID)
        {
            try
            {
                var application = _Applications.Find(x => x.applicationID == applicationID && x.Detail.uID == uID).FirstOrDefault();
                if (application != null)
                {
                    var output = application.Notes.Where(x => x.Detail.noteID == noteID).FirstOrDefault();
                    return output;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //___Contacts Related - Below___
        public async Task<Contact> GetContact(string uID, string applicationID, string contactID)
        {
            try
            {
                var application = _Applications.Find(x => x.applicationID == applicationID && x.Detail.uID == uID).FirstOrDefault();
                if (application != null)
                {
                    var output = application.Contacts.Where(x => x.Detail.contactID == contactID).FirstOrDefault();
                    return output;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //___Follow Ups Related - Below___
        public async Task<FollowUp> GetFollowUp(string uID, string applicationID, string followUpID)
        {
            try
            {
                var application = _Applications.Find(x => x.applicationID == applicationID && x.Detail.uID == uID).FirstOrDefault();
                if (application != null)
                {
                    var output = application.FollowUps.Where(x => x.Detail.followUpID == followUpID).FirstOrDefault();
                    return output;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___Checklists Related - Below___
        public async Task<Checklist> GetChecklist(string uID, string applicationID, string checklistID)
        {
            try
            {
                var application = _Applications.Find(x => x.applicationID == applicationID && x.Detail.uID == uID).FirstOrDefault();
                if (application != null)
                {
                    var output = application.Checklists.Where(x => x.checklistID == checklistID).FirstOrDefault();
                    return output;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}
