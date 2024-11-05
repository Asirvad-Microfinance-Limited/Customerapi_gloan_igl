using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class RelationProperties
    {
        [JsonProperty("relationId")]
        public long RELATION_ID { get; set; }
        [JsonProperty("relationName")]
        public string RELATION_NAME { get; set; }
    }
}
