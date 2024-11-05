using APIBaseClassLibrary.V1.Models.Response;
using KYCAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Response
{
    public class EKYCAdarResponse : BaseResponse
    {
        #region Declarations
        public List<EKYCCheckAdarProperties> ekycAdarList { get; set; }

        #endregion Declarations
    }

    public class EKYCAdarverifyUUIDResponse : BaseResponse
    {
        #region Declarations
        public List<EKYCVerifyUUIDProperties> ekycAdarVerifyList { get; set; }

        #endregion Declarations
    }
}
