using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class EKYCExistsProperties
    {
        #region Declarations

        [JsonProperty("custId")]
        public string CUST_ID { get; set; }

      
        #endregion Declarations
    }

    public class UUIDExistsProperties
    {
        #region Declarations

        [JsonProperty("custId")]
        public string CUST_ID { get; set; }


        #endregion Declarations
    }

    public class EKYCCheckAdarProperties
    {
        #region Declarations

        [JsonProperty("idNumber")]
        public string ID_NUMBER { get; set; }


        #endregion Declarations
    }

    public class EKYCVerifyCustProperties
    {
        #region Declarations

        [JsonProperty("uuId")]
        public string UUID { get; set; }

        [JsonProperty("verifiedDt")]
        public string VERIFIED_DT { get; set; }

        #endregion Declarations
    }

    public class EKYCVerifyUUIDProperties
    {
        #region Declarations

        [JsonProperty("uuId")]
        public string UUID { get; set; }

        [JsonProperty("verifiedDt")]
        public string VERIFIED_DT { get; set; }

        [JsonProperty("custId")]
        public string CUST_ID { get; set; }



        #endregion Declarations
    }

    public class KycIdTypesProperties
    {
        #region Declarations

        [JsonProperty("statusId")]
        public long STATUS_ID { get; set; }

        [JsonProperty("description")]
        public string DESCRIPTION { get; set; }


        #endregion Declarations
    }

   
}
