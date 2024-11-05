using APIBaseClassLibrary.V1.Models.Response;
using KYCAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Response
{
    public class KycIdTypesResponse : BaseResponse
    {
       
            #region Declarations
            public List<KycIdTypesProperties> generalList { get; set; }

            #endregion Declarations
        
    }
}
