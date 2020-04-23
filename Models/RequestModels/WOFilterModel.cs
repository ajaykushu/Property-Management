﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
    public class WOFilterModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PropertyName { get; set; }
        public string CreationStartDate { get; set; }
        public string CreationEndDate { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }

        [Range(-1, 4)]
        public int Priority { get; set; }

        public int PageNumber { get; set; }
    }
}