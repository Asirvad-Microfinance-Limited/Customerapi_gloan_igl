using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.BLL
{
    public  interface IGeneralBLL
    {
        BranchResponse getBranch(BranchRequest request);
        CountriesResponse getCountries();
        DateAndTimeResponse getDateAndTime();
        DetailsResponse getDetails(DetailsRequest detailsRequest);
        DistrictResponse getDistricts(DistrictRequest districtReques);
        GeneralParameterResponse getGeneralParameter(GeneralParameterRequest generalParameterRequest);
        PinCodeResponse getPinCode(PinCodeRequest pinCodeRequest);
        PostOfficeResponse getPostOffice(PostOfficeRequest postOfficeRequest);
        PreNameResponse getPreNameList();
        StatesResponset getStates(StatesRequest statesRequest);
        StatusResponse getStatusMaster(StatusRequest statusRequest);
        OccupationSubCategoryResponse getOccupationSubCategory(OccupationSubCategoryRequest OccupationSubCategoryRequest);
    }
}
