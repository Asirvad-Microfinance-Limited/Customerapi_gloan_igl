using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class CKYCCountriesProperties
    {
        [JsonProperty("countryId")]
        public long COUNTRY_ID { get; set; }
        [JsonProperty("countryName")]
        public string COUNTRY_NAME { get; set; }
    }
}
