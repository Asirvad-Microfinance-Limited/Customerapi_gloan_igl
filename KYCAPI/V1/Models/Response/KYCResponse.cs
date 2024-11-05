using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KYCAPI.V1.Models.Response
{
    public class KYCResponse : BaseResponse
    {
        #region Declarations
       /* public string consentData { get; set; }*/ // this is sample variable, must remove

        #endregion Declarations
    }

    public class VisaResponse : BaseResponse
    {
       
        
    }

    public class visadetailsGetResponse : BaseResponse
    {
        
        public string visaNumber { get; set; }
        public string visaIssueDate { get; set; }
        public string visaExpiryDate { get; set; }
        public string Country1 { get; set; }
        public string Country2 { get; set; }
        public string Country3 { get; set; }
        public string visaDocument { get; set; }

     }

    public class AadharConsentResponse : BaseResponse
    {


    }

    public class UploadAddressproofResponse : BaseResponse
    {


    }
    public class KycSelfCertifyResponse : BaseResponse
    {
        
    }
}
