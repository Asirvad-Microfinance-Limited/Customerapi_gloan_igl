using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class DistrictsProperties
    {
        [JsonProperty("districtId")]
        public long DISTRICT_ID { get; set; }
        [JsonProperty("districtName")]
        public string DISTRICT_NAME { get; set; }
    }
}
