using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class CustomerDetailsProperties
    {
        [JsonProperty("resident")]
        public long RESIDENT { get; set; }
        [JsonProperty("fathusPre")]
        public long FATHUS_PRE { get; set; }
        [JsonProperty("countryId")]
        public long COUNTRY_ID { get; set; }
    }
}
