using PhotoAPI.V1.Models.Request;
using PhotoAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAPI.V1.BLLDependency
{
    public interface IPhotoBLL
    {
        CustomerPhotoResponse getCustomerPhoto(CustomerPhotoRequest customerPhotoRequest);
        AddPhotoResponse addCustomerPhoto(AddPhotoRequest addPhotoRequest);
        AddNewCustomerPhotoResponse newcustomeraddPhoto(AddNewCustomerPhotoRequest addNewCustomerPhotoRequest);
        UpdatePhotoResponse updateCustomerPhoto(UpdatePhotoRequest updatePhotoRequest);
    }
}
