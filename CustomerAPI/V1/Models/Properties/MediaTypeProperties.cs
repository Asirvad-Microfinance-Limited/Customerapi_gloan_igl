using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class MediaTypeProperties
    {
        [JsonProperty("typeId")]
        public long TYPE_ID { get; set; }
        [JsonProperty("mediaType")]
        public string TYPE_NAME { get; set; }
    }
}
