using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class CountriesProperties
    {
        [JsonProperty("countryId")]
        public long COUNTRY_ID { get; set; }
        [JsonProperty("countryName")]
        public string COUNTRY_NAME { get; set; }
    }
}
