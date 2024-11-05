using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class EKYCCustomerProperties
    {
        [JsonProperty("countryName")]
        public long COUNTRY_ID { get; set; }
        [JsonProperty("stateId")]
        public long STATE_ID { get; set; }
        [JsonProperty("stateName")]
        public string STATE_NAME { get; set; }
        [JsonProperty("districtId")]
        public long DISTRICT_ID { get; set; }
        [JsonProperty("districtName")]
        public string DISTRICT_NAME { get; set; }


    }
}
