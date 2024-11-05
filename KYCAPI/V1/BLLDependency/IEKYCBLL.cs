using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.BLLDependency
{
    public interface IEKYCBLL
    {
        AadharDataResponse customerKYCDisplaydata(AadharDataRequest aadharDataRequest);
        AddAadhaarResponse addAadhaar(AddAadhaarRequest addAadhaarRequest);
        EKYCResponse EKYCGet(EKYCGetRequest eKYCGetRequest);
        EkycPostResponse updateEKYC(EKYCRequest eKYCRequest);     
        KycIdTypesResponse KYCIdTypesGet(string authToken);
        PawnBrokerResponse PawnBrokerGetImage(PawnBrokerRequest pawnBrokerRequest);
    }
}
