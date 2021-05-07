using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Configurations;
using TEAM_Server.Model.DB.Category;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Services.Service
{
    public class CategoryService : ICategoryService
    {
        public IMongoCollection<Category> _Categories;

        public ICategoryService _category;
        public CategoryService(
            IOptions<MongoDBSettings> settings,
            ICategoryService category)
        {
            _category = category;
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Categories = database.GetCollection<Category>(settings.Value.Categories);
        }

        //___________________________________________________________________________________
        //
        // Get Method Type Handlers - Below
        //___________________________________________________________________________________
        public async Task<String> GetCategoryIDByType(string Type)
        {
            try
            {
                var category = _Categories.Find(x => x.Type.ToLower().Equals(Type.ToLower()) || x.Type.ToLower().Contains(Type.ToLower())).FirstOrDefault();
                if(category != null)
                {
                    return category.categoryID;
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

        //___________________________________________________________________________________
        //
        // Update/Add Method Type Handlers - Below
        //___________________________________________________________________________________
        public async Task<Category> AddCategory(Category param)
        {
            try
            {
                var category = _Categories.Find(x => x.Type.ToLower() == param.Type.ToLower()).FirstOrDefault();
                if(category != null)
                {
                    List<Task> Tasks = new List<Task>();
                    //var update = Builders<Category>.Update.Set("SuggestionsOrSeleceted.$[i].obj3s.$[j].obj4s.$[k].id", "2cb66");
                   
                    if(param.SuggestionsOrSeleceted != null || param.SuggestionsOrSeleceted.Count != 0)
                    {
                        var filter = Builders<Category>.Filter.Eq(x => x.Type.ToLower(), param.Type.ToLower());
                        foreach (var item in param.SuggestionsOrSeleceted)
                        {
                            var update = Builders<Category>.Update.AddToSet(x => x.SuggestionsOrSeleceted, item);
                            Tasks.Add(_Categories.UpdateOneAsync(filter, update));
                        }
                    }
                }
                else
                {
                    param.categoryID = Guid.NewGuid().ToString();
                    if (param.SuggestionsOrSeleceted == null)
                        param.SuggestionsOrSeleceted = new List<Category_Suggestion>();
                    await _Categories.InsertOneAsync(param);
                }
                return param;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //___________________________________________________________________________________
        //
        // Initial Method Type Handlers - Below
        //___________________________________________________________________________________
        public async Task AddInitialCategory()
        {
            try
            {
                var list = new List<Category>
                {
                    new Category
                    {
                        categoryID = "allID",
                        Type = "All",
                        SuggestionsOrSeleceted = new List<Category_Suggestion>()
                    },
                    new Category
                    {
                        categoryID = "starredID",
                        Type = "Starred",
                        SuggestionsOrSeleceted = new List<Category_Suggestion>()
                    },
                    new Category
                    {
                        categoryID = "roleID",
                        Type = "Role",
                        SuggestionsOrSeleceted = new List<Category_Suggestion>()
                    },
                    new Category
                    {
                        categoryID = "locationID",
                        Type = "Location",
                        SuggestionsOrSeleceted = new List<Category_Suggestion>()
                    },
                };
                foreach (var item in list)
                {
                    _Categories.InsertOne(item);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
