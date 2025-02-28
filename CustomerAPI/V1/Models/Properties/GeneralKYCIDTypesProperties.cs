using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class GeneralKYCIDTypesProperties
    {
        [JsonProperty("KYCIDType")]
        public decimal identity_id { get; set; }
        [JsonProperty("Name")]
        public string identity_name { get; set; }
    }
}
