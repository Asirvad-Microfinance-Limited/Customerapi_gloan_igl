using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class DetailsProperties
    {
        public string countryId { get; set; }
        [JsonProperty("countryName")]
        public string COUNTRY_NAME { get; set; }


        public string stateId { get; set; }
        [JsonProperty("stateName")]
        public string STATE_NAME { get; set; }

        public string districtId { get; set; }
        [JsonProperty("districtName")]
        public string DISTRICT_NAME { get; set; }
        [JsonProperty("pinCode")]
        public long PIN_CODE { get; set; }

        public string postofficeId { get; set; }
        [JsonProperty("postOffice")]
        public string POST_OFFICE { get; set; }
       
       
        
    }
}
