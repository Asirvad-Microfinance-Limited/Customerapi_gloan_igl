using BankAPI.V1.Models.Request;
using BankAPI.V1.Models.Response;
using EmployeeAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.V1.BLL
{
    public interface IBankBLL
    {
        BankResponse getBankDetailsByIFSC(BankRequest request);
        BankFillResponse getBankList(BankFillRequest request);
        AddBankResponse addBankRequest(AddBankRequest request);
        getBankResponse getBankDetails(BankRequestCust request);
        AccountTypeResponse getAccountTypes();
        CustomerBankPhotoResponse CustomerBankPhotoGet(CustomerBankPhotoRequest customerBankPhotoRequest);
    }
}
