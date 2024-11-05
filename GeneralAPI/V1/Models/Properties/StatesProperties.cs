using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class StatesProperties
    {
        [JsonProperty("stateId")]
        public long STATE_ID { get; set; }
        [JsonProperty("stateName")]
        public string STATE_NAME { get; set; }
    }
}
