using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class EKYCAadharCustomerProperties
    {
        [JsonProperty("cusName")]
        public string CUS_NAME  { get; set; }
        [JsonProperty("cusDob")]
        public string CUS_DOB { get; set; }
        [JsonProperty("cusGender")]
        public string CUS_GENDER { get; set; }
        [JsonProperty("cusHouse")]
        public string CUS_HOUSE { get; set; }
        [JsonProperty("cusStreet")]
        public string CUS_STREET { get; set; }
        [JsonProperty("cusLocal")]
        public string CUS_LOCAL { get; set; }
        [JsonProperty("cusDist")]
        public string CUST_DIST { get; set; }
        [JsonProperty("cusState")]
        public string CUST_STATE { get; set; }
        [JsonProperty("cusMobile")]
        public string CUST_MOBILE { get; set; }
        [JsonProperty("cusMail")]
        public string CUST_MAIL { get; set; }
        [JsonProperty("cusFat")]
        public string CUST_FAT { get; set; }
        [JsonProperty("cusPin")]
        public string CUST_PIN { get; set; }

    }
}
