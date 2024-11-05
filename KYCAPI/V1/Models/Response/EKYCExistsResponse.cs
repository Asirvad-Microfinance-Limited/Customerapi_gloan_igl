using APIBaseClassLibrary.V1.Models.Response;
using KYCAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Response
{
    public class EKYCExistsResponse : BaseResponse
    {
        #region Declarations
        public List<EKYCExistsProperties> ekycExistsList { get; set; }

        #endregion Declarations
    }

   
    
    public class EKYCVerifyCustIdResponse : BaseResponse
    {
        #region Declarations
        public List<EKYCVerifyCustProperties> ekycCustVerifyList { get; set; }

        #endregion Declarations
    }

    public class getUUIDExistsResponse : BaseResponse
    {
        #region Declarations
        public List<UUIDExistsProperties> uuIdExistsList { get; set; }

        #endregion Declarations
    }
}
