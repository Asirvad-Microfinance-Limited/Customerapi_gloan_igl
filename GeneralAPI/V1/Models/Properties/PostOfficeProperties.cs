using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class PostOfficeProperties
    {


        [JsonProperty("postofficeId")]
     //   [JsonProperty("srNumber")]
        public long SR_NUMBER { get; set; }
        [JsonProperty("pinCode")]
        public long PIN_CODE { get; set; }
        [JsonProperty("postOffice")]
        public string POST_OFFICE { get; set; }
    }
}
