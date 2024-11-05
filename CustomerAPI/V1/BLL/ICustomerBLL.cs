using CustomerAPI.V1.Models;
using CustomerAPI.V1.Models.Request;
using CustomerAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.BLL
{
    public interface ICustomerBLL
    {
        CustomerResponse addCustomer(CustomerAddRequest customer);
        CustomerResponse updateCustomer(CustomerUpdateRequest customer);
        CustomerMasterDataResponse getMasterDataForCustomer();
        SearchCustomerResponse searchCustomer(SearchCustomerRequest searchCustomerRequest);
        DsaBaResponse getDsaBa(DsaBaRequest dsaBaRequest);
        CustomerDataResponse getCustomer(CustomerDataRequest customerDataRequest);
        DuplicateSearchCustomerResponse isDuplicateCustomer(DuplicateSearchCustomerRequest request);
        CustomerReferenceDetailsResponse AddCustometReferenceDetails(CustomerReferenceDetailsRequest request);
        matchingCustomerResponse MatchingCustomerList(matchingCustomerRequest request);
        UpdatePEPDetailsResponse UpdatePEPDetails(UpdatePEPDetailsRequest request);
    }
}
