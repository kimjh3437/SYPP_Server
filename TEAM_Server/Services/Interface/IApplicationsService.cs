using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Applications;
using TEAM_Server.Model.General.PrimitiveType;

namespace TEAM_Server.Services.Interface
{
    public interface IApplicationsService
    {
        Application CreateApplication(Application application);

        List<Application> GetApplications(List_Model list);
    }
}
