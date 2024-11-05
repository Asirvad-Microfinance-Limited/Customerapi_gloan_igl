using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class MediaMasterProperties
    {
        [JsonProperty("mediaId")]
        public long MEDIA_ID { get; set; }

        [JsonProperty("typeId")]
        public long TYPE_ID { get; set; }

        [JsonProperty("mediaName")]
        public string MEDIA_NAME { get; set; }
    }
}
