using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class CustomerPEPProperties
    {
        [JsonProperty("pepId")]
        public long PEP_ID { get; set; }
        [JsonProperty("pepDescription")]
        public string PEP_DESCRIPTION { get; set; }
    }
}
