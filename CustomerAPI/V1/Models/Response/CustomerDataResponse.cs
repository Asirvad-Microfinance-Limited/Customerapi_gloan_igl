using APIBaseClassLibrary.V1.Models.Response;
using CustomerAPI.V1.Models.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Response
{
    public class CustomerDataResponse : BaseResponse
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
        public string PIN_CODE { get; set; }
        [JsonProperty("phone2")]
        public string PHONE2 { get; set; }
        [JsonProperty("phone1")]
        public string PHONE1 { get; set; }
        [JsonProperty("districtName")]
        public string DISTRICT_NAME { get; set; }
        [JsonProperty("stateName")]
        public string STATE_NAME { get; set; }
        [JsonProperty("countryName")]
        public string COUNTRY_NAME { get; set; }
        [JsonProperty("identityId")]
        public string IDENTITY_ID { get; set; }
        [JsonProperty("identityName")]
        public string IDENTITY_NAME { get; set; }
        [JsonProperty("idNumber")]
        public string ID_NUMBER { get; set; }
        [JsonProperty("shareNo")]
        public string SHARE_NO { get; set; }
        [JsonProperty("addressProof")]
        public string ADDRESS_PROOF { get; set; }
        [JsonProperty("branchId")]
        public string BRANCH_ID { get; set; }
        [JsonProperty("firmId")]
        public string FIRM_ID { get; set; }
        [JsonProperty("isactive")]
        public string ISACTIVE { get; set; }
        
        [JsonProperty("motherName")]
        public string mother_name { get; set; }

        [JsonProperty("pinSerialNumber")]
        public long sr_number { get; set; }

        [JsonProperty("districtId")]
        public long district_id { get; set; }

        [JsonProperty("stateId")]
        public long state_id { get; set; }

        [JsonProperty("countryId")]
        public long country_id { get; set; }

        [JsonProperty("customerPreName")]
        public long name_pre { get; set; }

        [JsonProperty("cardNo")]
        public string  card_no { get; set; }

        [JsonProperty("prefLang")]
        public decimal Pref_lang { get; set; }

        [JsonProperty("occupationId")]
        public long occupation_id { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime date_of_birth { get; set; }

        [JsonProperty("custType")]
        public long cust_type { get; set; }

        [JsonProperty("emailId")]
        public string email_id { get; set; }

        [JsonProperty("gender")]
        public long gender { get; set; }

        [JsonProperty("citizen")]
        public long citizen { get; set; }

        [JsonProperty("registerDate")]
        public DateTime reg_date { get; set; }

        [JsonProperty("pan")]
        public string pan { get; set; }

        [JsonProperty("empCode")]
        public long emp_code { get; set; }

        [JsonProperty("landDtl")]
        public long land_dtls { get; set; }

        [JsonProperty("religion")]
        public long religion { get; set; }

        [JsonProperty("caste")]
        public long caste { get; set; }

        [JsonProperty("purposeofloan")]
        public long purposeofloan { get; set; }

        [JsonProperty("custStatus")]
        public long cust_status { get; set; }

        [JsonProperty("custCategory")]
        public long cust_category { get; set; }

        [JsonProperty("pepId")]
        public long pep_id { get; set; }
        
        [JsonProperty("citizenShip")]
        public long citizenship { get; set; }

        [JsonProperty("nationality")]
        public long nationality { get; set; }

        [JsonProperty("resident")]
        public long resident { get; set; }

        [JsonProperty("maritalStatus")]
        public long marital_stat { get; set; }

        [JsonProperty("fatherPreName")]
        public long FATHUS_PRE { get; set; }

        [JsonProperty("addressFlg")]
        public long addr_flg { get; set; }

        [JsonProperty("educationQual")]
        public long EDU_QUAL { get; set; }

        [JsonProperty("needForLoan")]
        public long NEED_FOR_LOAN { get; set; }

        [JsonProperty("income")]
        public long INCOME { get; set; }

        [JsonProperty("firstGl")]
        public long FIRST_GL { get; set; }
        
        [JsonProperty("issuedate")]
        public string issue_dt { get; set; }

        [JsonProperty("expiryDate")]
        public string exp_dt { get; set; }

        [JsonProperty("issuePlace")]
        public string issue_plce { get; set; }

    

        [JsonProperty("exserviceStatus")]
        public long exservice_status { get; set; }

        [JsonProperty("exserviceNumber")]
        public string pension_order { get; set; }
       
        [JsonProperty("kycOf")]
        public long kycof { get; set; }

        [JsonProperty("media_id")]
        public long media_id { get; set; }
        [JsonProperty("mediaType")]
        public long mediaType { get; set; }

        [JsonProperty("altHouseName")]
        public string ALT_HOUSE_NAME { get; set; }

        [JsonProperty("altLocality")]
        public string ALT_LOCALITY { get; set; }

        [JsonProperty("altPinSerial")]
        public string ALT_PIN_SERIAL { get; set; }

        [JsonProperty("kycIdType")]
        public string KYCIDTYPE { get; set; }

        public List<CustomerReferenceProperties> customerReferences { get; set; }

        [JsonProperty("facebookID")] /// added by sreerekha K 100006  on 14-feb-2020
        public string facebookID { get; set; }
        //kyc master directions implementation----done by Sreerekha 100006 on 7-mar-2020 
        [JsonProperty("addressProofBilldate")]
        public string addressProofBilldate { get; set; }

        [JsonProperty("addressProofType")]
        public int addressProofType { get; set; }

        //risk categorisation of customers---- 13-apr-2020---Sreerekha--100006
        [JsonProperty("businessCategory")]
        public int businessCategory { get; set; }

        [JsonProperty("businessSubCategory")]
        public int businessSubCategory { get; set; }

        [JsonProperty("riskCategory")]
        public string riskCategory { get; set; }

        [JsonProperty("CKYCNumber")]
        public string CKYCNumber { get; set; }

        [JsonProperty("weddingDate")]
        public string weddingDate { get; set; }

        [JsonProperty("PeriodicUpdation")]
        public int PeriodicUpdation { get; set; }
        
    }

    public class EmailResponse : BaseResponse // added for welcome message for customer registration--done by sreerekha 10006 --17-feb-2020
    {
        public string message { get; set; }
    }
}
