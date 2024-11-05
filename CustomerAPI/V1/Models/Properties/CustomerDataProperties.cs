using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Properties
{
    public class CustomerDataProperties
    {
        [JsonProperty("custId")]
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
        public long PIN_CODE { get; set; }
        [JsonProperty("phone2")]
        public string PHONE2 { get; set; }
        [JsonProperty("districtName")]
        public string DISTRICT_NAME { get; set; }
        [JsonProperty("stateName")]
        public string STATE_NAME { get; set; }
        [JsonProperty("countryName")]
        public string COUNTRY_NAME { get; set; }
        [JsonProperty("identityId")]
        public long IDENTITY_ID { get; set; }
        [JsonProperty("identityName")]
        public string IDENTITY_NAME { get; set; }
        [JsonProperty("idNumber")]
        public string ID_NUMBER { get; set; }
        [JsonProperty("shareNo")]
        public string SHARE_NO { get; set; }
        [JsonProperty("addressProof")]
        public string ADDRESS_PROOF { get; set; }
        [JsonProperty("branchId")]
        public long BRANCH_ID { get; set; }
        [JsonProperty("firmId")]
        public long FIRM_ID { get; set; }
        [JsonProperty("isactive")]
        public long ISACTIVE { get; set; }

    }


    public class CustomerReferenceProperties
    {
        [JsonProperty("relationType")]
        public string RELTYPE { get; set; }
        [JsonProperty("referenceName")]
        public string REFNAME { get; set; }
        [JsonProperty("mobile")]
        public string MOB { get; set; }

       
    }
}


    
