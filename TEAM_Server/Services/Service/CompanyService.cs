using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.DTO.Company;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Services.Service
{
    
    public class CompanyService : ICompanyService
    {
        private IMongoCollection<Company> _Companies;
        public CompanyService(
            IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Companies = database.GetCollection<Company>(settings.Value.Companies);
        }

        //___________________________________________________________________________________
        //
        // Create/Update Method Type Handlers - Below
        //___________________________________________________________________________________

        //___Companies Related - Below___
        public async Task<Company> CreateCompany(Company_Detail input)
        {
            try
            {
                var companyID = Guid.NewGuid().ToString();
                input.companyID = companyID;
                input.SubmittedTime = DateTime.UtcNow;
 
                var company = new Company
                {
                    Detail = input,
                    Events = new List<Event>(),
                    Notes = new List<Note>(),
                    Contacts = new List<Contact>(),
                    FollowUps = new List<FollowUp>(),
                    Checklists = new List<Checklist>()
                };
                await _Companies.InsertOneAsync(company);
                return company;
            }
            catch (Exception ex)
            {
                Console.Write($"<CreateApplication> : {ex}");
                return null;
            }
        }
        public async Task<bool> ChangeCompanyIsFavorite(Company_IsFavorite_Update_DTO model)
        {
            try
            {
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, model.companyID);
                var update = Builders<Company>.Update.Set(x => x.Detail.IsFavorite, model.IsFavorite);
                var result = await _Companies.UpdateOneAsync(filter, update);
                return result.IsAcknowledged;
            }
            catch(Exception ex)
            {
                return false;
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
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.AddToSet(x => x.Events, param);
                var result = await _Companies.UpdateOneAsync(filter, update);
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
        public async Task<Event> UpdateEvent(Event param)
        {
            try
            {
                var filter = Builders<Company>.Filter.Eq(x => x.Detail.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.Set("Events.$[event]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("event.eventID", param.eventID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Companies.UpdateOneAsync(filter, update, updateOptions);
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
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.AddToSet(x => x.Notes, param);
                var result = await _Companies.UpdateOneAsync(filter, update);
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
        public async Task<Note> UpdateNote(Note param)
        {
            try
            {
                var filter = Builders<Company>.Filter.Eq(x => x.Detail.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.Set("Notes.$[note]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("note.noteID", param.noteID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Companies.UpdateOneAsync(filter, update, updateOptions);
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
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.AddToSet(x => x.Contacts, param);
                var result = await _Companies.UpdateOneAsync(filter, update);
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
        public async Task<Contact> UpdateContact(Contact param)
        {
            try
            {
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.Set("Contacts.$[contact]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("contact.contactID", param.contactID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Companies.UpdateOneAsync(filter, update, updateOptions);
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
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.AddToSet(x => x.FollowUps, param);
                var result = await _Companies.UpdateOneAsync(filter, update);
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
        public async Task<FollowUp> UpdateConvoHistory(FollowUp param)
        {
            try
            {
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.Detail.companyID);
                var update = Builders<Company>.Update.Set("FollowUps.$[followup]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("followup.followUpID", param.followUpID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Companies.UpdateOneAsync(filter, update, updateOptions);
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
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.companyID);
                var update = Builders<Company>.Update.AddToSet(x => x.Checklists, param);
                var result = await _Companies.UpdateOneAsync(filter, update);
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
        public async Task<Checklist> UpdateChecklist(Checklist param)
        {
            try
            {
                var filter = Builders<Company>.Filter.Eq(x => x.companyID, param.companyID);
                var update = Builders<Company>.Update.Set("Checklists.$[checklist]", param);
                var arrayFilters = new List<ArrayFilterDefinition>();
                ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("checklist.checklistID", param.checklistID);

                arrayFilters.Add(level1);
                var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                var result = await _Companies.UpdateOneAsync(filter, update, updateOptions);
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

        //___Companies Related - Below___
        public async Task<List<Company>> GetCompanies(List<String> list)
        {
            try
            {
                List<Company> Companies = new List<Company>();
                Companies = _Companies.Find<Company>(x => list.Contains(x.companyID)).ToList();
                if (Companies != null)
                    return Companies;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //___Events Related - Below___
        public async Task<Event> GetEvent(string uID, string companyID, string eventID)
        {
            try
            {
                var company = _Companies.Find(x => x.companyID == companyID && x.Detail.uID == uID).FirstOrDefault();
                if(company != null)
                {
                    var output = company.Events.Where(x => x.Detail.eventID == eventID).FirstOrDefault();
                    return output;
                }
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        //___Notes Related - Below___
        public async Task<Note> GetNotes(string uID, string companyID, string noteID)
        {
            try
            {
                var company = _Companies.Find(x => x.companyID == companyID && x.Detail.uID == uID).FirstOrDefault();
                if (company != null)
                {
                    var output = company.Notes.Where(x => x.Detail.noteID == noteID).FirstOrDefault();
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
        public async Task<Contact> GetContacts(string uID, string companyID, string contactID)
        {
            try
            {
                var company = _Companies.Find(x => x.companyID == companyID && x.Detail.uID == uID).FirstOrDefault();
                if (company != null)
                {
                    var output = company.Contacts.Where(x => x.Detail.contactID == contactID).FirstOrDefault();
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
        public async Task<FollowUp> GetFollowUp(string uID, string companyID, string followUpID)
        {
            try
            {
                var company = _Companies.Find(x => x.companyID == companyID && x.Detail.uID == uID).FirstOrDefault();
                if (company != null)
                {
                    var output = company.FollowUps.Where(x => x.Detail.followUpID == followUpID).FirstOrDefault();
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
        public async Task<Checklist> GetChecklist(string uID, string companyID, string checklistID)
        {
            try
            {
                var company = _Companies.Find(x => x.companyID == companyID && x.Detail.uID == uID).FirstOrDefault();
                if (company != null)
                {
                    var output = company.Checklists.Where(x => x.checklistID == checklistID).FirstOrDefault();
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
