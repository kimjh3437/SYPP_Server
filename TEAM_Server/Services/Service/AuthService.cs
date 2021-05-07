using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TEAM_Server.Configurations;
using TEAM_Server.Model.DB.Auth;
using TEAM_Server.Model.DB.Category;
using TEAM_Server.Model.DB.Users;
using TEAM_Server.Model.Notification;
using TEAM_Server.Model.RestRequest.Auth;
using TEAM_Server.Services.Interface;
using TEAM_Server.Utilities.Notification;

namespace TEAM_Server.Services.Service
{
    public class AuthService  : IAuthService
    {
        private INotificationService _notification;
        private IMongoCollection<Auth> _auths;
        private IMongoCollection<User> _Users;
        private IOptions<MongoDBSettings> _settings;
        private IOptions<AppSettings> _secrets;
        public AuthService(
            INotificationService notification,
            IOptions<AppSettings> secrets,
            IOptions<MongoDBSettings> settings)
        {
            _secrets = secrets;
            _settings = settings;
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Users = database.GetCollection<User>(settings.Value.Users);
            _auths = database.GetCollection<Auth>(settings.Value.Auth);
            _notification = notification;
        }
        void UpdatePersonalTag(string uID, string tag, bool remove)
        {
            List<string> list = new List<string>();
            list.Add(tag);
            NotificationSubscription sub = _notification.GetInstallation(uID);
            DeviceInstallation install = sub.Installation;
            _notification.UpdateTag(list, install, remove, uID);
        }
        void Connector(string payload, string type, bool silence, string tag)
        {
            NotificationRequest request = new NotificationRequest();
            payload = payload.Replace("\"", "'");
            request.Contents = payload;
            request.Silent = silence;
            request.Tags = Array.Empty<string>();
            request.Action = type;
            if (tag != null)
            {
                string[] ar = new string[1];
                ar[0] = tag;
                request.Tags = ar;
            }
            _notification.RequestNotificationAsync(request);
        }

        //___________________________________________________________________________________
        //
        // Get Method Type Handlers - Below
        //___________________________________________________________________________________
        public async Task<List<User_Personal>> GetAllUsers()
        {
            List<User_Personal> personals = new List<User_Personal>();
            List<User> list = new List<User>();
            list = _Users.Find<User>(x => true).ToList();
            foreach (var user in list)
            {
                personals.Add(user.Personal);
            }
            if (personals != null)
                return personals;
            return null;

        }

        //___User  Related - Below___
        public async Task<List<string>> GetApplicationIDs(string uID)
        {
            try
            {
                var user = _Users.Find(x => x.uID == uID).FirstOrDefault();
                if(user != null)
                {
                    return user.ApplicationIDs;
                }
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<List<string>> GetcompanyIDs(string uID)
        {
            try
            {
                var user = _Users.Find(x => x.uID == uID).FirstOrDefault();
                if (user != null)
                {
                    return user.CompanyIDs;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<string>> GetTemplateIDs(string uID)
        {
            try
            {
                var user = _Users.Find(x => x.uID == uID).FirstOrDefault();
                if (user != null)
                {
                    return user.TemplateIDs;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //___Applications Related - Below___

        //___Events Related - Below___

        //___Notes Related - Below___

        //___Contacts Related - Below___

        //___Follow Ups Related - Below___

        //___Checklists Related - Below___


        //___________________________________________________________________________________
        //
        // Update Method Type Handlers - Below
        //___________________________________________________________________________________

        public async Task<bool> UpdatePersonalInfo(User_Personal personal)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.uID, personal.uID);
                var update = Builders<User>.Update.Set(x => x.Personal, personal);
                var result = await _Users.UpdateOneAsync(filter, update);
                return result.IsAcknowledged;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserApplicationID(string uID, string applicationID, bool isRemove)
        {
            try
            {
                if (!isRemove)
                {
                    var filter = Builders<User>.Filter.Eq(x => x.uID, uID);
                    var update = Builders<User>.Update.AddToSet(x => x.ApplicationIDs, applicationID);
                    var result = await _Users.UpdateOneAsync(filter, update);
                    return result.IsAcknowledged;
                }
                else
                {
                    var filter = Builders<User>.Filter.Eq(x => x.uID, uID);
                    var update = Builders<User>.Update.Pull(x => x.ApplicationIDs, applicationID);
                    var result = await _Users.UpdateOneAsync(filter, update);
                    return result.IsAcknowledged;
                }
                
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserCompanyID(string uID, string companyID, bool isRemove)
        {
            try
            {
                if (!isRemove)
                {
                    var filter = Builders<User>.Filter.Eq(x => x.uID, uID);
                    var update = Builders<User>.Update.AddToSet(x => x.CompanyIDs, companyID);
                    var result = await _Users.UpdateOneAsync(filter, update);
                    return result.IsAcknowledged;
                }
                else
                {
                    var filter = Builders<User>.Filter.Eq(x => x.uID, uID);
                    var update = Builders<User>.Update.Pull(x => x.CompanyIDs, companyID);
                    var result = await _Users.UpdateOneAsync(filter, update);
                    return result.IsAcknowledged;
                }
                    
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserTemplateID(string uID, string templateID, bool isRemove)
        {
            try
            {
                if (!isRemove)
                {
                    var filter = Builders<User>.Filter.Eq(x => x.uID, uID);
                    var update = Builders<User>.Update.AddToSet(x => x.TemplateIDs, templateID);
                    var result = await _Users.UpdateOneAsync(filter, update);
                    return result.IsAcknowledged;
                }
                else
                {
                    var filter = Builders<User>.Filter.Eq(x => x.uID, uID);
                    var update = Builders<User>.Update.Pull(x => x.TemplateIDs, templateID);
                    var result = await _Users.UpdateOneAsync(filter, update);
                    return result.IsAcknowledged;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        //___________________________________________________________________________________
        //
        // Create Method Type Handlers - Below
        //___________________________________________________________________________________

        //___Applications Related - Below___
        public async Task<bool> UpdateUserPreferences(Category category, string uID) 
        {
            try
            {
                var user = await _Users.Find(x => x.uID == uID).FirstOrDefaultAsync();
                if(user != null)
                {
                    var filter = Builders<User>.Filter.Eq(x => x.uID, uID);
                    if (user.Preferences.Where(x => x.Type.ToLower().Equals(category.Type.ToLower()) || x.Type.ToLower().Contains(category.Type.ToLower())).Any())
                    {
                        List<Task> Tasks = new List<Task>();
                        foreach (var item in category.SuggestionsOrSeleceted)
                        {
                            var update = Builders<User>.Update.Set("Preferences.$[preference].SuggestionsOrSeleceted", item);
                            var arrayFilters = new List<ArrayFilterDefinition>();
                            ArrayFilterDefinition<BsonDocument> level1 = new BsonDocument("preference.categoryID", category.categoryID);
                            arrayFilters.Add(level1);
                            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                            var result = await _Users.UpdateOneAsync(filter, update, updateOptions);
                            return result.IsAcknowledged;
                        }
                    }
                    else
                    {
                        var update = Builders<User>.Update.AddToSet(x => x.Preferences, category);
                        var result = await _Users.UpdateOneAsync(filter, update);
                        return result.IsAcknowledged;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                                
                return false;
            }
        }

        //___Applications Related - Below___

        //___Events Related - Below___

        //___Notes Related - Below___

        //___Contacts Related - Below___

        //___Follow Ups Related - Below___

        //___Checklists Related - Below___


        //___________________________________________________________________________________
        //
        // Event Handler Method Type Handlers - Below
        //___________________________________________________________________________________
        public Boolean NameCheck(string email)
        {
            Auth user = new Auth();
            user = _auths.Find<Auth>(x => x.Email == email).FirstOrDefault();
            if (user == null)
                return true;
            return false;
        }
        public async Task<User> Register(User_Register model)
        {
            var uID = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new ApplicationException("Password is required");

            if (_auths.Find(x => x.Email == model.Email) == null)
                throw new ApplicationException("Username \"" + model.Email + "\" is already taken");


            List<string> list = new List<string>();
            User_Personal personal = new User_Personal
            {
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                uID = uID
            };
            Auth serialized = new Auth();

            // sets up personal information on newly registered account 
            var user = new User
            {
                uID = uID,
                Personal = personal,
                ApplicationIDs = new List<string>(),
                TemplateIDs = new List<string>(),
                CompanyIDs = new List<string>(),
                Preferences = new List<Category> 
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
                },
                Token = ""
            };

            //sets up security hash salt 
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
            serialized.Email = model.Email;
            serialized.PasswordHash = passwordHash;
            serialized.PasswordSalt = passwordSalt;
            serialized.uID = uID;

            _auths.InsertOne(serialized);
            //user.Status = true;
            if (user != null)
                _Users.InsertOne(user);
            var user_personal = JsonConvert.SerializeObject(personal);
            Connector(user_personal, "User_Register", false, null);
            return user;
        }
        public User Authenticate(User_Authenticate authenticate)
        {
            var username = authenticate.Email;
            var password = authenticate.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var auth = _auths.Find<Auth>(x => x.Email == username).FirstOrDefault();


            if (auth == null)
                return null;
            if (!VerifyPasswordHash(password, auth.PasswordHash, auth.PasswordSalt))
                return null;

            var user = _Users.Find<User>(x => x.uID == auth.uID).FirstOrDefault();

            // autrhoization token 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secrets.Value.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", auth.uID.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            // token

            user.Token = tokenString;
            return user;
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
