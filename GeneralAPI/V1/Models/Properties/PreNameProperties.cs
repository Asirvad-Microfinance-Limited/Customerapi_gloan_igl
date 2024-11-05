using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class PreNameProperties
    {
        [JsonProperty("preNameId")]
        public long PRENAME_ID { get; set; }
        [JsonProperty("preName")]
        public string PRENAME { get; set; }
        [JsonProperty("gender")]
        public long GENDER { get; set; }
    }
}
