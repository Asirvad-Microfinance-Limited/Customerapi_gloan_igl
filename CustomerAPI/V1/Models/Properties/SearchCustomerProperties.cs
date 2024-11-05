using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class SearchCustomerProperties
    {
        [JsonProperty("cust_Id")]
        public string CUST_ID { get; set; }
        [JsonProperty("name")]
        public string NAME { get; set; }
        [JsonProperty("fatHus")]
        public string FAT_HUS { get; set; }
        [JsonProperty("houseName")]
        public string HOUSE_NAME { get; set; }
        [JsonProperty("locality")]
        public string LOCALITY { get; set; }
        [JsonProperty("postOffice")]
        public string POST_OFFICE { get; set; }
        [JsonProperty("pinCode")]
        public string PIN_CODE { get; set; }
        [JsonProperty("phone1")]
        public string PHONE1 { get; set; }
        [JsonProperty("phone2")]
        public string PHONE2 { get; set; }
        [JsonProperty("shareNo")]
        public string SHARE_NO { get; set; }
                
        [JsonProperty("statusId")]
        public decimal STATUSID { get; set; }

        [JsonProperty("status")]
        public string STATUS { get; set; }

        [JsonProperty("pep_id")]
        public string PEP_ID { get; set; }
    }
}
