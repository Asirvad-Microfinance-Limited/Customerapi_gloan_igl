using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Response
{
    public class OGLCustomerResponse : BaseResponse
    {
        public int REBATE_STATUS { get; set; }
        public string VER_ID { get; set; }
        public int BRANCH_ID { get; set; }
        public string CUST_ID { get; set; }
        public string CUST_NAME { get; set; }
        public int STATUS_ID { get; set; }
        public string INVENTORY { get; set; }
        public string SCHEME { get; set; }
        
    }
}
