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
    [Route("api/v1/states")]
    [ApiController]
    public class StatesController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public StatesController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get States details</summary>
        /// <param name="StatesRequest"><see cref="StatesRequest"/></param>
        /// <returns><see cref="StatesResponset"/></returns>
        [HttpGet]
        public ActionResult<StatesResponset> Get([FromQuery]StatesRequest statesRequest)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                StatesResponset statesResponset = new StatesResponset();
                statesResponset = generalBLL.getStates(statesRequest);
                return Ok(statesResponset);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}