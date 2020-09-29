using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Configurations;
using TEAM_Server.Model.Sample;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Services.Service
{
    public class OutputService : IOutputService
    {
        private IMongoCollection<Output> _Output;
        public OutputService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Output = database.GetCollection<Output>("Output");

        }

        public Output Insert(Output model)
        {
            _Output.InsertOne(model);
            return model; 
        }
    }
}
