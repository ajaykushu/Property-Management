using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
    public class EditWorkOrder
    {
        public long Id { get; set; }
        public List<SelectItem> Locations { get; set; }
        [Required]
        public int LocationId { get; set; }
        public List<SelectItem> SubLocations { get; set; }
        [Required]
        public int SubLocationId { get; set; }
        public List<SelectItem> Items { get; set; }

        [Required(ErrorMessage = "Please Select Item")]
        public int ItemId { get; set; }


        public string PropertyName { get; set; }

        public List<SelectItem> Issues { get; set; }

        [Required(ErrorMessage = "Please Select Issue Id")]
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Please give Date")]
        public DateTime CreatedDate { get; set; }
        public List<KeyValuePair<string, string>> FileAvailable { get; set; }
        

        public List<SelectItem> Departments { get; set; }

        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Please give Section Id")]
        public long UserId { get; set; }
        public DateTime DueDate { get; set; }
        public List<SelectItem> Users { get; set; }
        public string FilesRemoved { get; set; }
        public int Priority { get; set; }
        // public IFormFileCollection File { get; set; }
    }
}