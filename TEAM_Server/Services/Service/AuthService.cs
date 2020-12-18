using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    public class AuthService : IAuthService
    {
        private INotificationService _notification;
        IMongoCollection<Auth> _auths;
        private IMongoCollection<User> _Users;
        private IOptions<MongoDBSettings> _settings;
        private IOptions<AppSettings> _secrets;
        public AuthService(
            IOptions<AppSettings> secrets,
            IOptions<MongoDBSettings> settings)
        {
            _secrets = secrets;
            _settings = settings;
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Users = database.GetCollection<User>(settings.Value.Users);
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

        public List<User_Personal> GetAllUsers()
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

        public Boolean NameCheck(string email)
        {
            Auth user = new Auth();
            user = _auths.Find<Auth>(x => x.Email == email).FirstOrDefault();
            if (user == null)
                return true;
            return false;
        }


        public User Register(User_Register model)
        {
            var uID = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new ApplicationException("Password is required");

            if (_auths.Find(x => x.Email == model.Email) != null)
                throw new ApplicationException("Username \"" + model.Email + "\" is already taken");

            User user = new User();
            List<string> list = new List<string>();
            User_Personal personal = new User_Personal
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                uID = uID
            };
            Auth serialized = new Auth();

            // sets up personal information on newly registered account 
            user.uID = uID;
            user.authID = uID;
            user.Personal = personal;
            user.ApplicationIDs = new List<string>();
            user.TemplateIDs = new List<string>();
            user.CompanyIDs = new List<string>();
            user.Preferences = new List<Category>();

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

        public bool UpdatePersonalInfo(User_Personal personal)
        {
            var user = _Users.Find<User>(x => x.uID == personal.uID).FirstOrDefault();

            var filter = Builders<User>.Filter.Eq(x => x.uID, personal.uID);
            var update = Builders<User>.Update.Set(x => x.Personal, personal);
            var status = _Users.UpdateOne(filter, update);
            if (status != null)
            {
                var json = JsonConvert.SerializeObject(personal);
                Connector(json, "User_Update", false, null);
                return true;
            }
            return false;

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
