using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KYCAPI.V1.Models.Response
{
    public class ConsentResponse : BaseResponse
    {
        #region Declarations
        public string consentData { get; set; }

        #endregion Declarations
    }

    public class addConsentResponse : BaseResponse
    {
        #region Declarations

        [JsonProperty("message")]
        public string message { get; set; }

        #endregion Declarations

    }

}
