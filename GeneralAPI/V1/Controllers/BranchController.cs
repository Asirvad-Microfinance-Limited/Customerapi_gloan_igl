using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using GeneralAPI.V1.BLL;
using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeneralAPI.V1.Controllers
{
    [Route("api/v1/branch")]
    [ApiController]
    public class BranchController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public BranchController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }


        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get branch details </summary>
        /// <param name="BranchRequest"><see cref="BranchRequest"/></param>
        /// <returns><see cref="BranchResponse"/></returns>
        [HttpGet]
        public ActionResult<BranchResponse> Get([FromQuery]BranchRequest branchRequest)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                BranchResponse branchResponse = new BranchResponse();
                branchResponse = generalBLL.getBranch(branchRequest);
            return Ok(branchResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}