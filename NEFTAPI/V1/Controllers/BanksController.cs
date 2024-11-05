using APIBaseClassLibrary.V1.Controllers;
using BankAPI.V1.BLL;
using BankAPI.V1.Models.Request;
using BankAPI.V1.Models.Response;
using EmployeeAPI.V1.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BankAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BanksController : BaseController
    {
        IBankBLL bank;
        IConfiguration configuration;
        public BanksController(IBankBLL _bank, IConfiguration iConfig)
        {
            bank = _bank;
            configuration = iConfig;
        }

        #region BankGet

        [HttpGet]
        public ActionResult<BankResponse> Get([FromQuery]BankRequest request)
        {
            if (ModelState.IsValid)
            {
                //BankBLL bank = new BankBLL();
                return Ok(bank.getBankDetailsByIFSC(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion BankGet
        #region BankGetlist

        [HttpGet("banklist")]
        public ActionResult<BankFillResponse> Get([FromQuery]BankFillRequest request)
        {
            if (ModelState.IsValid)
            {
                //BankBLL bank = new BankBLL();
                return Ok(bank.getBankList(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

#endregion BankGetlist


        #region BankPost

        [HttpPost]
        public ActionResult<BankResponse> Post([FromBody] AddBankRequest request)
        {
            if (ModelState.IsValid)
            {
                //BankBLL bank = new BankBLL();
                return Ok(bank.addBankRequest(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion BankPost

    }
}