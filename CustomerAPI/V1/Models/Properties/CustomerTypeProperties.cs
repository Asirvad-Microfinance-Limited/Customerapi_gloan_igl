using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class CustomerTypeProperties
    {
        [JsonProperty("typeId")]
        public long TYPE_ID { get; set; }
        [JsonProperty("customerType")]
        public string DESCR { get; set; }
    }
}
