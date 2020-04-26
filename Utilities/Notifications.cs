using DataAccessLayer.Interfaces;
using DataEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities.Interface;

namespace Utilities
{
    public class Notifications : INotifier
    {
        private readonly IRepo<Notification> _notification;
        public Notifications(IRepo<Notification> notification)
        {
            _notification = notification;
        }
        public async Task<bool> CreateNotification(string message, List<long> AssignedTo, long navigator,char Type)
        {
            Notification notification = new Notification
            {
                Message = message,
                NavigatorId = navigator,
                NotificationType = Type.ToString(),
            };
            notification.UserNotification = new List<UserNotification>();
            foreach (var id in AssignedTo)
            {
                notification.UserNotification.Add(new UserNotification
                {
                    ApplicationUserId = id,
                    IsRead = false
                });
            }
           var status= await _notification.Add(notification);
            if (status > 0)
                return true;
            return false;


        }
    }
}
