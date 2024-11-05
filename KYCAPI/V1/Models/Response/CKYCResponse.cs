using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KYCAPI.V1.Models.Response
{
    public class CKYCResponse : BaseResponse
    {
        #region Declarations
        public string consentData { get; set; } // this is sample variable, must remove

        #endregion Declarations
    }
}
