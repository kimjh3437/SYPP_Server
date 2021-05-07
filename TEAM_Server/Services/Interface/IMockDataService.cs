using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.DB.Companies;
using TEAM_Server.Model.DB.Templates;
using TEAM_Server.Model.DB.Users;

namespace TEAM_Server.Services.Interface
{
    public interface IMockDataService
    {
        Task<User> GetUser();
        Task CreateDummyUser();
        Task LoadApplication();
        Task<List<Template>> GetTemplates();
        Task<List<Company>> GetCompanies();
        Task<List<Application>> GetApplications();
        Task<Boolean> CreateDummyModels();
        Template CreateDummyTemplate(string title);
        List<Word_Template> CreateWords();
    }
}
