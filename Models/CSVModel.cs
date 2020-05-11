using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CSVModel
    {
        public string Id { get; set; }
        public string Due_Date { get; set; }
        public string Created_Time { get; set; }
        public string Created_By_User_Name { get; set; }
        public string Updated_By_User_Name { get; set; }
        public string Updated_Time { get; set; }
        public string Sub_Location { get; set; }
        public string Assigned_To { get; set; }
        public string Description { get; set; }
        public string Issue_Name { get; set; }
        public string Priority { get; set; }
        public string Property_Name { get; set; }
        public string Location { get; set; }
        public string Stage_Code { get; set; }
        public string Stage_Description { get; set; }
        public List<string> Attachment { get; set; }
    }
}
