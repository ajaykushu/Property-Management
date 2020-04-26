using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataEntity
{
    public class UserNotification
    {
        [Key]
        public long Id { get; set; }
        public bool IsRead { get; set; }
        public long NotificationId { get; set; }
        public Notification Notification { get; set; }
        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
