using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TEAM_Server.Model.Notification;
using TEAM_Server.Utilities.Notification;

namespace TEAM_Server.Services.Interface
{
    public interface INotificationService
    {
        Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken token);
        Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token);
        Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest);
        Task<bool> UpdateTag(List<string> tags, DeviceInstallation device, bool remove, string uID);
        NotificationSubscription GetInstallation(string uID);
        bool UploadInstallation(NotificationSubscription model);
    }
}
