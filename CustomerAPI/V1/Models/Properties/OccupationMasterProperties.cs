using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class OccupationMasterProperties
    {
        [JsonProperty("occupationId")]
        public long OCCUPATION_ID { get; set; }

        [JsonProperty("occupationName")]
        public string OCCUPATION_NAME { get; set; }
    }
}
