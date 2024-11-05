using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class CKYCStatusMasterProperties
    {
        public long STATUS_ID { get; set; }
        public long MODULE_ID { get; set; }
        public long OPTION_ID { get; set; }
        public long ORDER_BY { get; set; }
        public decimal TYPE { get; set; }
        public string DESCRIPTION { get; set; }
        
    }
}
