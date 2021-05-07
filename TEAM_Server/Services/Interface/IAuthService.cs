using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Category;
using TEAM_Server.Model.DB.Users;
using TEAM_Server.Model.RestRequest.Auth;

namespace TEAM_Server.Services.Interface
{
    public interface IAuthService
    {
        //___________________________________________________________________________________
        //
        // Get Method Type Handlers - Below
        //___________________________________________________________________________________
        Task<List<User_Personal>> GetAllUsers();
        Task<List<string>> GetApplicationIDs(string uID);
        Task<List<string>> GetcompanyIDs(string uID);
        Task<List<string>> GetTemplateIDs(string uID);


        //___________________________________________________________________________________
        //
        // Update Method Type Handlers - Below
        //___________________________________________________________________________________
        Task<bool> UpdatePersonalInfo(User_Personal personal);
        Task<bool> UpdateUserApplicationID(string uID, string applicationID, bool isRemove);
        Task<bool> UpdateUserCompanyID(string uID, string companyID, bool isRemove);
        Task<bool> UpdateUserTemplateID(string uID, string templateID, bool isRemove);

        //___________________________________________________________________________________
        //
        // Crete Method Type Handlers - Below
        //___________________________________________________________________________________
        Task<bool> UpdateUserPreferences(Category category, string uID);


        //___________________________________________________________________________________
        //
        // Event Handler Method Type Handlers - Below
        //___________________________________________________________________________________
        Boolean NameCheck(string email);
        Task<User> Register(User_Register model);
        User Authenticate(User_Authenticate authenticate);

    }
}
