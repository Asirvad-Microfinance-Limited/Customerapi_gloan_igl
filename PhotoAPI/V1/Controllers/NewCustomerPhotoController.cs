using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhotoAPI.V1.BLL;
using PhotoAPI.V1.BLLDependency;
using PhotoAPI.V1.Models.Request;
using PhotoAPI.V1.Models.Response;

namespace PhotoAPI.V1.Controllers
{
    [Route("api/v1/newcustomerphoto")]
    [ApiController]
    public class NewCustomerPhotoController : BaseController
    {
        IConfiguration configuration;
        IPhotoBLL photo;
        public NewCustomerPhotoController(IConfiguration iConfig, IPhotoBLL _photoBll)
        {
            photo = _photoBll;
            configuration = iConfig;
        }

        #region NewCustomerPhotoPost

        [HttpPost]
        public ActionResult<AddNewCustomerPhotoResponse> Post([FromBody]AddNewCustomerPhotoRequest addNewCustomerPhotoRequest)
        {
            if (ModelState.IsValid)
            {
                //PhotoBLL mobileBLL = new PhotoBLL();
                AddNewCustomerPhotoResponse addPhotoResponse = new AddNewCustomerPhotoResponse();
                addPhotoResponse = photo.newcustomeraddPhoto(addNewCustomerPhotoRequest);
                return Ok(addPhotoResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion NewCustomerPhotoPost
    }
}