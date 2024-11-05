using APIBaseClassLibrary.V1.Controllers;
using BankAPI.V1.BLL;
using BankAPI.V1.Models.Request;
using BankAPI.V1.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BankAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerBankController : BaseController
    {
        IBankBLL bank;
        IConfiguration configuration;
        public CustomerBankController(IBankBLL _bank, IConfiguration iConfig)
        {
            bank = _bank;
            configuration = iConfig;
        }

        #region BankcustGet

        [HttpGet]
        public ActionResult<getBankResponse> Get([FromQuery]BankRequestCust request)
        {
            if (ModelState.IsValid)
            {
                //BankBLL bank = new BankBLL();
                return Ok(bank.getBankDetails(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion BankGet



        #region BankcustPhotoGet

        [HttpGet("bankPhoto")]
        public ActionResult<CustomerBankPhotoResponse> GetPhoto([FromQuery]CustomerBankPhotoRequest customerBankPhotoRequest)
        {
            if (ModelState.IsValid)
            {
                //BankBLL bank = new BankBLL();
                return Ok(bank.CustomerBankPhotoGet(customerBankPhotoRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion BankcustPhotoGet

      


    }
}