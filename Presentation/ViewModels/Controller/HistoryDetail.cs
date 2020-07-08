using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class HistoryDetail
    {
            [DisplayName("Property Name")]
            public string PropertyName { get; set; }
        [DisplayName("Old Name")]
        public string OldValue { get; set; }
        [DisplayName("New Value")]
        public string NewValue { get; set; }
        [DisplayName("Comment")]
        public string Comment { get; set; }
        [DisplayName("Updated Time")]
        public string UpdateTime { get; set; }
        [DisplayName("Updated By")]
        public string UpdatedBy { get; set; }
        
    }
}
