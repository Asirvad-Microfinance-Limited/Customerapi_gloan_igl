using APIBaseClassLibrary.V1.Models.Response;
using KYCAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KYCAPI.V1.Models.Response
{
    public class EKYCResponse : BaseResponse
    {
        #region Declarations
        public List<EKYCExistsProperties> ekycExistsList { get; set; }

        public List<EKYCVerifyCustProperties> ekycCustVerifyList { get; set; }

        public List<EKYCCheckAdarProperties> ekycAdarList { get; set; }

        public List<EKYCVerifyUUIDProperties> ekycAdarVerifyList { get; set; }

        public List<UUIDExistsProperties> uuIdExistsList { get; set; }

        #endregion Declarations


    }

    public class EKYCCountResponse : BaseResponse
    {
        #region Declarations
        public long ekycCount { get; set; }

        #endregion Declarations

    }

    public class EkycPostResponse : BaseResponse
    {
        #region Declarations

        [JsonProperty("message")]
        public string message { get; set; }

        #endregion Declarations

    }

    public class MastersApiResponse<T> : BaseResponse
    {
        #region Declarations
        public List<T> statusList { get; set; }

        #endregion Declarations
    }
}
