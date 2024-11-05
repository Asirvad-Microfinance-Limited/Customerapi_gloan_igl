using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class StatusMasterProperties
    {
     
        public long STATUS_ID { get; set; }
        public long MODULE_ID { get; set; }
        public long OPTION_ID { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
