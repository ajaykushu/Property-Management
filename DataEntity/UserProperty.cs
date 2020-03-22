﻿using System.ComponentModel.DataAnnotations;

namespace DataEntity
{
    public class UserProperty
    {
        [Key]
        public long Id { get; set; }
        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public long PropertyId { get; set; }
        public Property Property { get; set; }
    }
}