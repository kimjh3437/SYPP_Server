using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.Sample;

namespace TEAM_Server.Services.Interface
{
    public interface IOutputService
    {
        public Output Insert(Output model);
    }
}
