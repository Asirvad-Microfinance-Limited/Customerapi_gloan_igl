using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KYCAPI.V1.BLL;
using KYCAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KYCAPI.V1.Models.Request;
using APIBaseClassLibrary.V1.Controllers;
using Microsoft.Extensions.Configuration;
using KYCAPI.V1.BLLDependency;

namespace KYCAPI.V1.Controllers
{
    [Route("api/v1/kyc")]
    public class KYCController : BaseController
    {

        IConfiguration configuration;
        IKYCBLL ckyc;
        public KYCController(IConfiguration iConfig, IKYCBLL _ckyc)
        {
            configuration = iConfig;
            ckyc = _ckyc;
        }
        [HttpGet]
        public ActionResult<KYCDocResponse> Get(KYCDocRequest request)
        {
            if (ModelState.IsValid)
            {
               // KYCBLL ckyc = new KYCBLL();
                return Ok(ckyc.getKYCDoc(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("getVisadetails")]
        public ActionResult<visadetailsGetResponse> Get(visadetailsGetRequest request)
        {
            if (ModelState.IsValid)
            {
               
                return Ok(ckyc.getVisaDetails(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/CKYC
        //[HttpPost]
        //public ActionResult<int> Post(test request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        KYCBLL ckyc = new KYCBLL();


        //        return Ok(request.id);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}


        // POST: api/CKYC
        [HttpPost]
        public ActionResult<KYCResponse> Post([FromBody] KYCRequest request)
        {
            if (ModelState.IsValid)
            {
                //KYCBLL ckyc = new KYCBLL();
                return Ok(ckyc.addKyc(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        // GET: api/CKYC
        // [HttpPut]
        //public ActionResult<KYCResponse> Put(KYCRequest request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        KYCBLL ckyc = new KYCBLL();
        //        return Ok(ckyc.updateKycDocuments(request));
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}

        //public class test
        //{
        //    public int id { get; set; }
        //}

        //Adding visa details of customers wef 10-dec-2019 done by Sreerekha K 100006
        [HttpPost("AddVisaDetails")]
        public ActionResult<VisaResponse> Post([FromBody] VisaRequest request)
        {
            if (ModelState.IsValid)
            {

                return Ok(ckyc.addVisaDetails(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("UploadAadharConsent")]
        public ActionResult<AadharConsentResponse> Post([FromBody] AadharConsentRequest request)
        {
            if (ModelState.IsValid)
            {

                return Ok(ckyc.uploadAadharConsent(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet("getAadharConsent")]
        public ActionResult<AdharDocResponse> Get(AdharDocRequest request)
        {
            if (ModelState.IsValid)
            {
               
                return Ok(ckyc.getAadharDoc(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        //--------------KYC Master directions implementations---------------
        [HttpPost("UploadAddressProof")]
        public ActionResult<UploadAddressproofResponse> Post([FromBody] UploadAddressproofRequest request)
        {
            if (ModelState.IsValid)
            {

                return Ok(ckyc.uploadAddressProof(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("getAddressProofDocument")]
        public ActionResult<AddressProofGetResponse>Get(AdressProofGetRequest request)

        {
            if (ModelState.IsValid)
            {

                return Ok(ckyc.getAddressProofDoc(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //De dupe based on KYC ID--17-oct-2020-----------------------
        [HttpGet("KYCdeDupe")]
        public ActionResult<KycDeDupeResponse> Get(KycDeDupeRequest request)

        {
            if (ModelState.IsValid)
            {

                return Ok(ckyc.getKYCdeDupe(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        //De dupe based on KYC ID-------------------------

        //KYC control implementation 2nd phase 12-Jan-2021
        [HttpPost("UploadKycSelfCertify")]
        public ActionResult<KycSelfCertifyResponse> KycSelfCertify([FromBody]KycSelfCertifyRequest kycSelfCertifyRequest)
        {
            if (ModelState.IsValid)
            {
               return Ok(ckyc.KycSelfCertify(kycSelfCertifyRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        //KYC control implementation 2nd phase 12-Jan-2021
    }
}
