using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class GeneralPreNameProperties
    {
        [JsonProperty("preNameId")]
        public long PRENAME_ID { get; set; }
        [JsonProperty("preName")]
        public string PRENAME { get; set; }
        [JsonProperty("gender")]
        public long GENDER { get; set; }
    }
}
