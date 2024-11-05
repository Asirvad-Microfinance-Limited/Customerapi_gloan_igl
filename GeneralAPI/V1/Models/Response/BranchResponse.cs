using APIBaseClassLibrary.V1.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Response
{
    public class BranchResponse : BaseResponse
    {
        public BranchResponse()
        {
            BRANCH_ID = -1;
        } 

        [JsonProperty("branchId")]
        public long BRANCH_ID { get; set; }
        [JsonProperty("branchName")]
        public string BRANCH_NAME { get; set; }
        [JsonProperty("districtName")]
        public string DISTRICT_NAME { get; set; }
        [JsonProperty("stateName")]
        public string STATE_NAME { get; set; }
        [JsonProperty("phone1")]
        public string PHONE1 { get; set; }
        [JsonProperty("phone2")]
        public string PHONE2 { get; set; }
        [JsonProperty("branchAddress")]
        public string BRANCH_ADDR { get; set; }
        [JsonProperty("pinCode")]
        public long PINCODE { get; set; }
        [JsonProperty("firmName")]
        public string FIRM_NAME { get; set; }
    }
}
