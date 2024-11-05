using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class CasteMasterProperties
    {
        [JsonProperty("casteId")]
        public decimal CASTE_ID { get; set; }

        [JsonProperty("religionId")]
        public decimal RELIGION_ID { get; set; }

        [JsonProperty("casteName")]
        public string CASTE_NAME { get; set; }
    }
}
