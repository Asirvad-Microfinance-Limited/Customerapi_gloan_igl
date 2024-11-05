using APIBaseClassLibrary.V1.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.V1.Models.Response
{
    public class BankResponse : BaseResponse
    {
        #region Declarations

        [JsonProperty("stateId")]
        public long STATEID  { get; set; }
        [JsonProperty("stateName")]
        public string STATENAME { get; set; }
        [JsonProperty("districtId")]
        public long DISTRICTID { get; set; }
        [JsonProperty("districtName")]
        public string DISTRICTNAME { get; set; }
        [JsonProperty("ifscCode")]
        public string IFSCCODE { get; set; }
        [JsonProperty("bankId")]
        public long BANKID { get; set; }
        [JsonProperty("bankName")]
        public string BANKNAME { get; set; }

        #endregion Declarations


    }

    public class getBankResponse : BaseResponse
    {

        #region Declarations

        [JsonProperty("ifscCode")]
        public string IFSCCODE { get; set; }
        [JsonProperty("bankName")]
        public string BANKNAME { get; set; }
        [JsonProperty("bankBranch")]
        public string BANKBRANCH { get; set; }
        [JsonProperty("bankAccount")]
        public string BANKACCOUNT { get; set; }
        [JsonProperty("accountName")]
        public string ACCOUNTNAME { get; set; }
        [JsonProperty("bankID")]
        public long BANKID { get; set; }
        [JsonProperty("custID")]
        public string CUSTID { get; set; }
        [JsonProperty("serialNO")]
        public long SERIALNO { get; set; } // this is sample variable, must remove
        [JsonProperty("accStatus")]
        public string ACCSTATUS { get; set; }
        [JsonProperty("custName")]
       
        public string CUSTNAME { get; set; }
        [JsonProperty("stateID")]
       
        public long STATEID { get; set; }
        [JsonProperty("stateName")]
      
        public string STATENAME { get; set; }
        [JsonProperty("districtID")]
        
        public long DISTRICTID { get; set; }
        [JsonProperty("districtName")]
      
        public string DISTRICTNAME { get; set; }

        [JsonProperty("accountType")]
        public long ACCOUNTTYPE { get; set; }

        #endregion Declarations
    }


    public class CustomerBankPhotoResponse : BaseResponse
    {
        #region Declarations
        public string idProof { get; set; }
        public string tiffIdProof { get; set; }
        #endregion Declarations
    }

}
