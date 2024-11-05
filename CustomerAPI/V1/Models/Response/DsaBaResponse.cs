using APIBaseClassLibrary.V1.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Response
{
    public class DsaBaResponse: BaseResponse
    {
        [JsonProperty("brid")]
        public string BRID { get; set; }
        [JsonProperty("baCode")]
        public string BA_CODE { get; set; }
        [JsonProperty("branchName")]
        public string BRANCH_NAME { get; set; }
        [JsonProperty("baName")]
        public string BA_NAME { get; set; }
        [JsonProperty("address")]
        public string ADDRESS { get; set; }
        [JsonProperty("phone")]
        public string PHONE { get; set; }
        [JsonProperty("email")]
        public string EMAIL { get; set; }

    }
}
