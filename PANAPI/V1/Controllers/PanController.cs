using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PANAPI.V1.BLL;
using PANAPI.V1.BLLDependency;
using PANAPI.V1.Models.Request;
using PANAPI.V1.Models.Response;

namespace PANAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PanController : BaseController
    {
        IConfiguration configuration;
        IPanBLL pan;
        public PanController(IConfiguration iConfig, IPanBLL _panBLL)
        {
            pan = _panBLL;
            configuration = iConfig;
        }
        #region Pan

        #region PanGet

        [HttpGet]
        public ActionResult<PanDetailsResponse> Get([FromQuery]PanDetailsRequest panDetailsRequest)
        {
            if (ModelState.IsValid)
            {
             //   PanBLL panBLL = new PanBLL();
                return Ok(pan.getPanDetails(panDetailsRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion PanGet

        #region PanPut

        [HttpPut]
        public ActionResult<UpdatePanResponse> Put([FromBody]UpdatePanDetailsRequest updatePanDetailsRequest)
        {
            if (ModelState.IsValid)
            {
               // PanBLL panBLL = new PanBLL();
                return Ok(pan.updatePan(updatePanDetailsRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion PanPut

        #endregion Pan

        #region Form60
        [HttpPost("Uploadform60")]
        public ActionResult<UpdateForm60Response> Post([FromBody]UpdateForm60Request updateForm60Request)
        {

            if (ModelState.IsValid)
            {
                
                return Ok(pan.updateForm60(updateForm60Request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
       
        [HttpGet("Getform60Image")]
        public ActionResult<Form60DetailsResponse> Get([FromQuery]Form60DeatilsRequest form60DetailsRequest)
        {
            if (ModelState.IsValid)
            {
                //   PanBLL panBLL = new PanBLL();
                return Ok(pan.getForm60Details(form60DetailsRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("SaveForm60Details")]
        public ActionResult<SaveForm60DetailsResponse> Post([FromBody]SaveForm60DetailsRequest saveForm60Request)
        {

            if (ModelState.IsValid)
            {

                return Ok(pan.SaveForm60Details(saveForm60Request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion Form60
    }
}