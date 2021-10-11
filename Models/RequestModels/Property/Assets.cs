using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Property.RequestModels
{
  
        public class Assets
        {
            public long AssetId { get; set; }
            public string AssetName { get; set; }

            List<AssignedProperties> Properties { get; set; }


        }
        public class AssignedProperties
        {
            public int Id { get; set; }
            public string Address { get; set; }
            public string Location { get; set; }
            public string Sublocation { get; set; }
        }
    }


