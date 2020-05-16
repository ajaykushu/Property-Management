using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Notification : Log
    {
        [Key]
        public long NId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Message { get; set; }

        public string NavigatorId { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string NotificationType { get; set; }//WA,WE,CA,RA,UA,UE,PA,PE,PC

        public virtual ICollection<UserNotification> UserNotification { get; set; }
    }
}