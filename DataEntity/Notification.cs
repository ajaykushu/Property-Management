﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class Notification:Log
    {
        [Key]
        public long NId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Message { get; set; }
        public long NavigatorId { get; set; }
        [Column(TypeName = "varchar(1)")]
        public string NotificationType { get; set; }//W,C,R,U,P
        public virtual ICollection<UserNotification> UserNotification { get; set; }
    }
}