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
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;
using TEAM_Server.Model.General.PrimitiveType;
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
                if (application.Categories == null)
                {
                    application.Categories = new List<Category>();
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
    }
}
