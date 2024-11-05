using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.BLLDependency
{
    public interface ICKYCBLL
    {
      
        CKYCResponse addCKYCDetails(CKYCRequest request);
        CKYCMasterDataResponse getCKYCDetails(CKYCMasterDataRequest cKYCMasterDataRequest);
    }
}
