using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PANAPI.V1.Models.Properties
{
    public class PanProperties
    {

        [JsonProperty("pan")]
        public string PAN { get; set; }

        [JsonProperty("custId")]
        public string CUST_ID { get; set; }

        [JsonProperty("panCopy")]
        public string PAN_COPY { get; set; }
     
    }
}
