using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TEAM_Server.Configurations;
using TEAM_Server.Model.Notification;
using TEAM_Server.Services.Interface;
using TEAM_Server.Utilities.Notification;

namespace TEAM_Server.Services.Service
{
    public class NotificationService : INotificationService
    {
        private IOptions<NotificationSettings> _settings;
        private IMongoCollection<NotificationSubscription> _installation;
        readonly NotificationHubClient _hub;
        readonly Dictionary<string, NotificationPlatform> _installationPlatform;
        public NotificationService(
            IOptions<MongoDBSettings> mongo,
            IOptions<NotificationSettings> settings)
        {
            var client = new MongoClient(mongo.Value.ConnectionString);
            var database = client.GetDatabase(mongo.Value.DatabaseName); ;
            _installation = database.GetCollection<NotificationSubscription>(mongo.Value.Notifications);
            _settings = settings;
            // _logger = logger;
            _hub = NotificationHubClient.CreateClientFromConnectionString(
                settings.Value.ConnectionString,
                settings.Value.HubName);

            _installationPlatform = new Dictionary<string, NotificationPlatform>
            {
                { nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns },
                { nameof(NotificationPlatform.Fcm).ToLower(), NotificationPlatform.Fcm },
                { nameof(NotificationPlatform.Wns).ToLower(), NotificationPlatform.Wns }
            };
        }

        public NotificationSubscription GetInstallation(string uID)
        {
            var installation = _installation.Find<NotificationSubscription>(x => x.uID == uID).FirstOrDefault();
            if (installation != null)
            {
                return installation;
            }
            return null;
        }

        public async Task<bool> UpdateTag(List<string> tags, DeviceInstallation device, bool remove, string uID)
        {
            if (device.InstallationId == null)
                return false;
            try
            {
                List<string> _tags = new List<string>();
                var installation = _hub.GetInstallation(device.InstallationId);
                if (!remove)
                {
                    if (installation.Tags != null)
                    {
                        foreach (var i in installation.Tags)
                        {
                            _tags.Add(i);
                        }
                    }
                    if (tags != null)
                    {
                        foreach (var i in tags)
                        {
                            if (!_tags.Contains(i))
                                _tags.Add(i);
                        }
                    }
                    var newtag = _tags.ToArray();
                    installation.Tags = newtag;
                }
                else
                {
                    if (tags != null)
                    {
                        foreach (var item in installation.Tags)
                        {
                            if (!tags.Contains(item))
                                _tags.Add(item);
                        }
                        var newtag = _tags.ToArray();
                        installation.Tags = newtag;
                    }
                }
                await _hub.CreateOrUpdateInstallationAsync(installation);
                device.Tags = _tags;
                NotificationSubscription sub = new NotificationSubscription
                {
                    uID = uID,
                    Installation = device
                };
                var status = UploadInstallation(sub);

                return status;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool UploadInstallation(NotificationSubscription model)
        {
            try
            {
                var ins = _installation.Find<NotificationSubscription>(x => x.uID == model.uID).FirstOrDefault();
                if (ins == null)
                {
                    if (model != null)
                    {
                        NotificationSubscription sub = new NotificationSubscription
                        {
                            uID = model.uID,
                            Installation = model.Installation
                        };
                        _installation.InsertOne(sub);
                    }
                }
                else
                {
                    var filter = Builders<NotificationSubscription>.Filter.Eq(x => x.uID, model.uID);
                    var update = Builders<NotificationSubscription>.Update.Set(x => x.Installation, model.Installation);
                    var status = _installation.UpdateOne(filter, update);
                    if (status != null)
                        return true;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(deviceInstallation?.InstallationId) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.Platform) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.PushChannel))
                return false;
            var installation = new Installation()
            {
                InstallationId = deviceInstallation.InstallationId,
                PushChannel = deviceInstallation.PushChannel,
                Tags = deviceInstallation.Tags
            };
            if (_installationPlatform.TryGetValue(deviceInstallation.Platform, out var platform))
                installation.Platform = platform;
            else
                return false;
            try
            {
                await _hub.CreateOrUpdateInstallationAsync(installation, token);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(installationId))
                return false;
            try
            {
                await _hub.DeleteInstallationAsync(installationId, token);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest)
        {

            if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                (string.IsNullOrWhiteSpace(notificationRequest?.Contents)) ||
                string.IsNullOrWhiteSpace(notificationRequest?.Action)))
                return false;

            var windowPushTemplate = PushTemplate.Generic.Windows;
            var androidPushTemplate = notificationRequest.Silent ?
                PushTemplate.Silent.Android :
                PushTemplate.Generic.Android;
            var iOSPushTemplate = notificationRequest.Silent ?
                PushTemplate.Silent.iOS :
                PushTemplate.Generic.iOS;
            var windowPayload = PrepareNotificationPayload(
                windowPushTemplate,
                notificationRequest.Contents,
                notificationRequest.Action);
            var androidPayload = PrepareNotificationPayload(
                androidPushTemplate,
                notificationRequest.Contents,
                notificationRequest.Action);
            var iOSPayload = PrepareNotificationPayload(
                iOSPushTemplate,
                notificationRequest.Contents,
                notificationRequest.Action);
            try
            {
                if (notificationRequest.Tags.Length == 0)
                {
                    // This will broadcast to all users registered in the notification hub
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, windowPayload);
                }
                else if (notificationRequest.Tags.Length <= 20)
                {
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, notificationRequest.Tags);
                    //TODO Fix Payload format 
                }
                else
                {
                    var notificationTasks = notificationRequest.Tags
                        .Select((value, index) => (value, index))
                        .GroupBy(g => g.index / 20, i => i.value)
                        .Select(tags => SendPlatformNotificationsAsync(androidPayload, iOSPayload, tags));

                    await Task.WhenAll(notificationTasks);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        string PrepareNotificationPayload(string template, string text, string action) => template
            .Replace("$(alertMessage)", text, StringComparison.InvariantCulture)
            .Replace("$(alertAction)", action, StringComparison.InvariantCulture);

        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, string windowPayload)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmNativeNotificationAsync(androidPayload),
                _hub.SendAppleNativeNotificationAsync(iOSPayload)
            };

            return Task.WhenAll(sendTasks);
        }
        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, IEnumerable<string> tags)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmNativeNotificationAsync(androidPayload, tags), //initially had token 
                _hub.SendAppleNativeNotificationAsync(iOSPayload, tags)
            };

            return Task.WhenAll(sendTasks);
        }
    }
}
