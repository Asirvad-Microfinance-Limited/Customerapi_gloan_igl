using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.BLLDependency
{
    public interface IKYCBLL
    {
        KYCDocResponse getKYCDoc(KYCDocRequest request);
        KYCResponse addKyc(KYCRequest request);
        VisaResponse addVisaDetails(VisaRequest request);
        visadetailsGetResponse getVisaDetails(visadetailsGetRequest request);
        AadharConsentResponse uploadAadharConsent(AadharConsentRequest request);
        AdharDocResponse getAadharDoc(AdharDocRequest request);
        UploadAddressproofResponse uploadAddressProof(UploadAddressproofRequest request);
        AddressProofGetResponse getAddressProofDoc(AdressProofGetRequest request);
        KycDeDupeResponse getKYCdeDupe(KycDeDupeRequest request);
        KycSelfCertifyResponse KycSelfCertify(KycSelfCertifyRequest kycSelfCertifyRequest);

    }
}
