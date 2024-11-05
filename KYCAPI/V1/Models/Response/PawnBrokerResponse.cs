using APIBaseClassLibrary.V1.Models.Response;
using KYCAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Response
{
    public class PawnBrokerResponse : BaseResponse
    {
        #region Declarations
        public List<PawnBrokerFormProperties> formImageList { get; set; }

        public List<PawnBrokerLicenseProperties> licenseImageList { get; set; }

        #endregion Declarations
    }


}
