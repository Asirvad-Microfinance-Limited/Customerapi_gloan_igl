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
    [Route("api/v1/statusmaster")]
    [ApiController]
    public class StatusMasterController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public StatusMasterController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }



        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get StatusMaster</summary>
        /// <param name="StatusRequest"><see cref="StatusRequest"/></param>
        /// <returns><see cref="StatusResponse"/></returns>
        [HttpGet]
        public ActionResult<StatusResponse> Get([FromQuery]StatusRequest statusRequest)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                StatusResponse statusResponse = new StatusResponse();
                statusResponse = generalBLL.getStatusMaster(statusRequest);
                return Ok(statusResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}