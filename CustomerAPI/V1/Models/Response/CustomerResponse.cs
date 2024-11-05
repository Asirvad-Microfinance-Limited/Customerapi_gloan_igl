using APIBaseClassLibrary.V1.Models.Response;

namespace CustomerAPI.V1.Models
{
    public class CustomerResponse : BaseResponse
    {

        public CustomerResponse()
        {
           
        }

        public string customerID { get; set; }

    }

    public class CustomerReferenceDetailsResponse : BaseResponse
    {

    

    }
}
