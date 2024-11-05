using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class ReligionProperties
    {
        [JsonProperty("religionId")]
        public long RELIGION_ID { get; set; }
        [JsonProperty("religion")]
        public string RELIGION { get; set; }
    }
}
