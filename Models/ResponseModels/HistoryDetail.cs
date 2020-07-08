using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ResponseModels
{
    public class HistoryDetail
    {
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Comment { get; set; }
        public string UpdateTime { get; set; }
        public string UpdatedBy { get; set; }
    }
}
