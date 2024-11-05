using APIBaseClassLibrary.V1.Models.Response;
using CustomerAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Response
{
    public class CustomerMasterDataResponse: BaseResponse
    {
        public List<CustomerStatusProperties> customerStatusList { get; set; }
        public List<CustomerTypeProperties> customerTypesList { get; set; }
        public List<MediaTypeProperties> mediaTypesList { get; set; }
        public List<MediaMasterProperties> mediaMasterList { get; set; }
        public List<ReligionProperties> religionsList { get; set; }
        public List<CasteMasterProperties> casteMasterList { get; set; }
        public List<GeneralStatusProperties> EducationalQualificationList { get; set; }
        public List<CustomerPEPProperties> customerPepsList { get; set; }
        public List<RelationProperties> relationsList { get; set; }
        public List<OccupationMasterProperties> businessList { get; set; }
        public List<GeneralStatusProperties> incomeList { get; set; }
        public List<GeneralStatusProperties> maritalStatus { get; set; }
        public List<GeneralStatusProperties> citizenships { get; set; }
        public List<GeneralStatusProperties> nationality { get; set; }
        public List<GeneralStatusProperties> residentialStatus { get; set; }
        public List<GeneralStatusProperties> languages { get; set; }
        public List<GeneralStatusProperties> addressProofs { get; set; }
        public List<GeneralStatusProperties> LoanReason { get; set; }
        public List<GeneralPreNameProperties> preNameList { get; set; }
        public List<GeneralKYCIDTypesProperties> kycIDTypes { get; set; }
        public List<GeneralKYCIDTypesProperties> interimKyc { get; set; }
        public List<GeneralKYCIDTypesProperties> moneyTransferKyc { get; set; }
    }

    public class UpdatePEPDetailsResponse : BaseResponse
    {



    }
}
