using APIBaseClassLibrary.V1.Models.Response;
using KYCAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Response
{
    public class AadharDataResponse:BaseResponse
    {
        #region Declarations
        public EKYCAadharCustomerProperties eKYCAadharCustomer { get; set; }
        public EKYCCustomerProperties eKYCCustomer { get; set; }
        public EKYCPostProperties eKYCPostOffice { get; set; }

        #endregion Declarations
    }
}
