using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class StatusProperties
    {
        [JsonProperty("statusId")]
        public long STATUS_ID { get; set; }
        [JsonProperty("Description")]
        public string DESCRIPTION { get; set; }
    }
}
