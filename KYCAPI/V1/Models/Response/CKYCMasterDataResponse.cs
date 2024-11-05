using APIBaseClassLibrary.V1.Models.Response;
using KYCAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Response
{
    public class CKYCMasterDataResponse : BaseResponse
    {
        #region Declarations
        public List<CKYCPStatusProperties> namePFXList { get; set; }
        public List<CKYCPStatusProperties> fatSpouseList { get; set; }
        public List<CKYCPStatusProperties> malePFXList { get; set; }
        public List<CKYCPStatusProperties> femalePFXList { get; set; }
        public List<CKYCPStatusProperties> spousePFXList { get; set; }
        public List<CKYCPStatusProperties> addresstypeList { get; set; }
        public List<CKYCCountriesProperties> countryList { get; set; }
        public CustomerDetailsProperties customerDetails { get; set; }
        public CKYCCustomerDetailsProperties CKYCcustomerDetails { get; set; }

        #endregion Declarations
    }
}
