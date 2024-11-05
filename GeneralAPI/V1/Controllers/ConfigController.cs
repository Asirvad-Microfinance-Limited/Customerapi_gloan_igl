using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using GeneralAPI.V1.BLL;
using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeneralAPI.V1.Controllers
{
    [Route("api/v1/config")]
    public class ConfigController : BaseController
    {
        IConfigBLL ConfigBLL;
        IConfiguration configuration;
        public ConfigController(IConfigBLL _ConfigBLL, IConfiguration iConfig)
        {
            ConfigBLL = _ConfigBLL;
            configuration = iConfig;
        }




        [HttpGet]
        public ActionResult<ConfigDetailsResponse> Get()
        {
            if (ModelState.IsValid)
            {
                
                ConfigDetailsResponse response = new ConfigDetailsResponse();
                response = ConfigBLL.getConfig();
                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public ActionResult<ConfigInsertResponse> Post(ConfigInsertRequest request)
        {
            if (ModelState.IsValid)
            {
               
                ConfigInsertResponse response = new ConfigInsertResponse();
                response = ConfigBLL.insertConfig(request);
                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public ActionResult<ConfigUpdateResponse> Put(ConfigUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
               
                ConfigUpdateResponse response = new ConfigUpdateResponse();
                response = ConfigBLL.updateConfig(request);
                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}