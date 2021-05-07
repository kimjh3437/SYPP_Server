using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Category;

namespace TEAM_Server.Services.Interface
{
    public interface ICategoryService
    {
        Task<String> GetCategoryIDByType(string Type);
        Task<Category> AddCategory(Category param);
        Task AddInitialCategory();
    }
}
