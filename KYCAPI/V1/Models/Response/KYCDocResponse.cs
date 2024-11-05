using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Response
{
    public class KYCDocResponse:BaseResponse
    {
        public string kycDoc { get; set; }
        public string tiffDoc { get; set; }
    }

    public class AdharDocResponse : BaseResponse
    {
        public string aadharConsent { get; set; }
        
    }

    public class AddressProofGetResponse : BaseResponse

    {
        public string aadharConsent { get; set; }

    }

    public class KycDeDupeResponse : BaseResponse

    {
        public string message { get; set; }

    }
}
