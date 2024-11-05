using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using BankAPI.V1.BLL;
using BankAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BankAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypesController : BaseController
    {
        IBankBLL bank;
        IConfiguration configuration;
        public AccountTypesController(IBankBLL _bank, IConfiguration iConfig)
        {
            bank = _bank;
            configuration = iConfig;
        }

        #region AccountTypesGet

        [HttpGet]
        public ActionResult<AccountTypeResponse> Get()
        {
            if (ModelState.IsValid)
            {
               
                return Ok(bank.getAccountTypes());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        #endregion AccountTypesGet

    }
}