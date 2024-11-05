using Microsoft.AspNetCore.Mvc;
using CustomerAPI.V1.Models;
using CustomerAPI.V1.Models.Request;
using CustomerAPI.V1.BLL;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CustomerAPI.V1.Models.Response;
using APIBaseClassLibrary.V1.Controllers;
using Microsoft.Extensions.Configuration;

namespace CustomerAPI.V1.Controllers
{
    [Route("api/v1/customer")]
    public class CustomerController : BaseController
    {
        ICustomerBLL customerBLL;
        IConfiguration configuration;
        public CustomerController(ICustomerBLL _customerBLL, IConfiguration iConfig)
        {
            customerBLL = _customerBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get Customer Details</summary>
        /// <param name="CustomerDataRequest"><see cref="CustomerDataRequest"/></param>
        /// <returns><see cref="CustomerDataResponse"/></returns>

        [HttpGet]
        public ActionResult<CustomerDataResponse> Get([FromQuery]CustomerDataRequest customerDataRequest)
        {
            if (ModelState.IsValid)
            {
                //CustomerBLL customerBLL = new CustomerBLL();
                CustomerDataResponse customerDataResponse = new CustomerDataResponse();
                customerDataResponse = customerBLL.getCustomer(customerDataRequest);
                return Ok(customerDataResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Post api/customer
        [HttpPost]
        public ActionResult<CustomerResponse> Post([FromBody]CustomerAddRequest customer)
        {
            if (ModelState.IsValid)
            {
                //CustomerBLL objCustomer = new CustomerBLL();
                return Ok(customerBLL.addCustomer(customer));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Post api/customer
        [HttpPost("referencedetails")]
        public ActionResult<CustomerReferenceDetailsResponse> PostReferenceDetails([FromBody]CustomerReferenceDetailsRequest customer)
        {
            if (ModelState.IsValid)
            {
               // CustomerBLL objCustomer = new CustomerBLL();
                return Ok(customerBLL.AddCustometReferenceDetails(customer));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Post api/customer
        [HttpPut]
        public ActionResult<CustomerResponse> Put([FromBody]CustomerUpdateRequest customer)
        {
            if (ModelState.IsValid)
            {
                //CustomerBLL objCustomer = new CustomerBLL();
                return Ok(customerBLL.updateCustomer(customer));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("isDuplicateCustomer")]
        public ActionResult<SearchCustomerResponse> isDuplicateCustomer([FromQuery]DuplicateSearchCustomerRequest customerDataRequest)
        {
            if (ModelState.IsValid)
            {
                //CustomerBLL customerBLL = new CustomerBLL();
                DuplicateSearchCustomerResponse customerDataResponse = new DuplicateSearchCustomerResponse();
                customerDataResponse = customerBLL.isDuplicateCustomer(customerDataRequest);
                return Ok(customerDataResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("matchingCustomerList")]
        public ActionResult<matchingCustomerResponse> getmatchingCustomerList([FromQuery]matchingCustomerRequest customerDataRequest)
        {
            if (ModelState.IsValid)
            {
                //CustomerBLL customerBLL = new CustomerBLL();
                matchingCustomerResponse customerDataResponse = new matchingCustomerResponse();
                customerDataResponse = customerBLL.MatchingCustomerList(customerDataRequest);
                return Ok(customerDataResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    

    }
}