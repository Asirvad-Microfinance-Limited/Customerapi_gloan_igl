using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GeneralAPI.V1.Models.Properties;
using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using GlobalValues;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;
namespace GeneralAPI.V1.BLL
{
    public class GeneralBLL : APIBaseBLL, IGeneralBLL
    {

        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public GeneralBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get branch details </summary>
        /// <param name="BranchRequest"><see cref="BranchRequest"/></param>
        /// <returns><see cref="BranchResponse"/></returns>
        public BranchResponse getBranch(BranchRequest branchRequest)
        {
          //DBAccessHelper helper = new DBAccessHelper();


            BranchResponse branchResponse = new BranchResponse();
            try
            {



                var query = "select a.branch_id,a.branch_name,d.district_name,s.state_name ,a.phone1,a.phone2,a.branch_addr,a.pincode,f.firm_name" +
                            " from branch_master a,district_master d, state_master s,firm_master f" +
                            " where a.branch_id = " + branchRequest.branchId + " and d.district_id = a.district_id and d.state_id = s.state_id and f.firm_id = a.firm_id";
                BranchResponse branch = DapperHelper.GetRecord<BranchResponse>(query,SQLMode.Query,null);
                if (branch.BRANCH_ID>-1)
                {
                    branchResponse =branch;
                    branchResponse.status.code = APIStatus.success;
                    branchResponse.status.message = "Success";
                    branchResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    branchResponse.status.code = APIStatus.no_Data_Found;
                    branchResponse.status.message = "No Data Found";
                    branchResponse.status.flag = ProcessStatus.success;
                }


            }
            catch (Exception ex)
            {
                branchResponse.status.code = APIStatus.exception;
                branchResponse.status.message = ex.Message;
                branchResponse.status.flag = ProcessStatus.failed;

            }

            return branchResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get country details </summary>
        /// <returns><see cref="BranchResponse"/></returns>
        public CountriesResponse getCountries()
        {
          //DBAccessHelper helper = new DBAccessHelper();
            CountriesResponse countriesResponse = new CountriesResponse();

            try
            {

                var query = "select country_id,country_name from country_dtl  order by country_id";
                var countries = DapperHelper.GetRecords<CountriesProperties>(query,SQLMode.Query,null);
                if (countries.Count > 0)
                {
                    countriesResponse.countriesList = countries;
                    countriesResponse.status.code = APIStatus.success;
                    countriesResponse.status.message = "Success";
                    countriesResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    countriesResponse.status.code = APIStatus.no_Data_Found;
                    countriesResponse.status.message = "No Data Found";
                    countriesResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                countriesResponse.status.flag = ProcessStatus.failed;
                countriesResponse.status.code = APIStatus.exception;
                countriesResponse.status.message = ex.Message;
            }
            return countriesResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get db DateAndTime </summary>
        /// <returns><see cref="DateAndTimeResponse"/></returns>
        public DateAndTimeResponse getDateAndTime()
        {
          //DBAccessHelper helper = new DBAccessHelper();
            DateAndTimeResponse dateAndTimeResponse = new DateAndTimeResponse();
            try
            {

                var query = "select sysdate from dual";
                DateTime obj = helper.ExecuteScalar<DateTime>(query);

                if (obj != null)
                {
                    //
                    dateAndTimeResponse.SYSDATE = obj.ToString();
                    dateAndTimeResponse.status.code = APIStatus.success;
                    dateAndTimeResponse.status.message = "Success";
                    dateAndTimeResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    dateAndTimeResponse.status.code = APIStatus.no_Data_Found;
                    dateAndTimeResponse.status.message = "No Data Found";
                    dateAndTimeResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                dateAndTimeResponse.status.code = APIStatus.exception;
                dateAndTimeResponse.status.message = ex.Message;
                dateAndTimeResponse.status.flag = ProcessStatus.failed;

            }

            return dateAndTimeResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get pincode details </summary>
        /// <param name="DetailsRequest"><see cref="DetailsRequest"/></param>
        /// <returns><see cref="DetailsResponse"/></returns>
        public DetailsResponse getDetails(DetailsRequest detailsRequest)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            DetailsResponse detailsResponse = new DetailsResponse();

            try
            {
                if (detailsRequest.pinCode.ToString().Length == 6) {

                    var query = "select pm.pin_code, to_char(pm.sr_number) as postofficeId, pm.post_office, to_char(dm.district_id) as districtId, " +
                                " dm.district_name,to_char(sm.state_id) as stateId, sm.state_name, to_char(cd.country_id) as countryId,cd.country_name " +
                                " from post_master pm, district_master dm, state_master sm, country_dtl cd where pm.pin_code = " + detailsRequest.pinCode + "" +
                                " and dm.district_id = pm.district_id and sm.state_id = dm.state_id and cd.country_id = sm.country_id  order by pm.post_office ";

                                     var details = DapperHelper.GetRecords<DetailsProperties>(query,SQLMode.Query,null);
                    if (details.Count > 0)
                    {
                        detailsResponse.detailsList = details;
                        detailsResponse.status.code = APIStatus.success;
                        detailsResponse.status.message = "Success";
                        detailsResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        detailsResponse.status.code = APIStatus.no_Data_Found;
                        detailsResponse.status.message = "No Data Found";
                        detailsResponse.status.flag = ProcessStatus.success;
                    }
                }
                else
                {
                    detailsResponse.status.code = APIStatus.validationFailed;
                    detailsResponse.status.message = "Invalid pin code.";
                    detailsResponse.status.flag = ProcessStatus.success;
                }
                    
            }
            catch (Exception ex)
            {
                detailsResponse.status.code = APIStatus.exception;
                detailsResponse.status.message = ex.Message;
                detailsResponse.status.flag = ProcessStatus.failed;

            }

            return detailsResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get districts details </summary>
        /// <param name="DistrictRequest"><see cref="DistrictRequest"/></param>
        /// <returns><see cref="DistrictResponse"/></returns>
        public DistrictResponse getDistricts(DistrictRequest districtReques)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            DistrictResponse districtResponse = new DistrictResponse();

            try
            {
                var query = "select district_id,district_name from district_master where state_id=" + districtReques.stateId + "  order by district_name";


                var districtsList = DapperHelper.GetRecords<DistrictsProperties>(query,SQLMode.Query,null);
                if (districtsList.Count > 0)
                {
                    districtResponse.districtsList = districtsList;
                    districtResponse.status.code = APIStatus.success;
                    districtResponse.status.message = "Success";
                    districtResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    districtResponse.status.code = APIStatus.no_Data_Found;
                    districtResponse.status.message = "No Data Found";
                    districtResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                districtResponse.status.code = APIStatus.exception;
                districtResponse.status.message = ex.Message;
                districtResponse.status.flag = ProcessStatus.failed;

            }

            return districtResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get General Parameter details</summary>
        /// <param name="GeneralParameterRequest"><see cref="GeneralParameterRequest"/></param>
        /// <returns><see cref="GeneralParameterResponse"/></returns>
        public GeneralParameterResponse getGeneralParameter(GeneralParameterRequest generalParameterRequest)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            GeneralParameterResponse generalParameterResponse = new GeneralParameterResponse();

            try
            {
                var query = "select parmtr_name,parmtr_value from general_parameter where firm_id = " + generalParameterRequest.firmId + " and parmtr_id = " + generalParameterRequest.parameterId + " and module_id = " + generalParameterRequest.moduleId + " order by parmtr_name";
                var generalParameterList = DapperHelper.GetRecords<GeneralParameterProperties>(query,SQLMode.Query,null);
                if (generalParameterList.Count > 0)
                {
                    generalParameterResponse.generalParameterList = generalParameterList;
                    generalParameterResponse.status.code = APIStatus.success;
                    generalParameterResponse.status.message = "Success";
                    generalParameterResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    generalParameterResponse.status.code = APIStatus.no_Data_Found;
                    generalParameterResponse.status.message = "No Data Found";
                    generalParameterResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                generalParameterResponse.status.code = APIStatus.exception;
                generalParameterResponse.status.message = ex.Message;
                generalParameterResponse.status.flag = ProcessStatus.failed;

            }

            return generalParameterResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get PinCode</summary>
        /// <param name="PinCodeRequest"><see cref="PinCodeRequest"/></param>
        /// <returns><see cref="PinCodeResponse"/></returns>
        public PinCodeResponse getPinCode(PinCodeRequest pinCodeRequest)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            PinCodeResponse pinCodeResponse = new PinCodeResponse();
            try
            {

                var query = "select pin_code  from post_master where sr_number =" + pinCodeRequest.postOfficeId + " and  status_id = 1";

                int pinCode = helper.ExecuteScalar<int>(query);

                if (pinCode>0)
                {
                    //
                    pinCodeResponse.pinCode = Convert.ToInt64(pinCode);
                    var objDetailsList = getpinserialDetails(pinCodeRequest.postOfficeId).detailsList;
                    if (objDetailsList != null)
                    {
                        pinCodeResponse.detailsList = objDetailsList;
                    }
                    pinCodeResponse.status.code = APIStatus.success;
                    pinCodeResponse.status.message = "Success";
                    pinCodeResponse.status.flag = ProcessStatus.success;
                }else
                {
                    pinCodeResponse.status.code = APIStatus.no_Data_Found;
                    pinCodeResponse.status.message = "No Data Found";
                    pinCodeResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                pinCodeResponse.status.code = APIStatus.exception;
                pinCodeResponse.status.message = ex.Message;
                pinCodeResponse.status.flag = ProcessStatus.failed;
            }

            return pinCodeResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get PostOffice details</summary>
        /// <param name="PostOfficeRequest"><see cref="PostOfficeRequest"/></param>
        /// <returns><see cref="PostOfficeResponse"/></returns>
        public PostOfficeResponse getPostOffice(PostOfficeRequest postOfficeRequest)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            PostOfficeResponse postOfficeResponse = new PostOfficeResponse();

            try
            {
                var query = "select pin_code ,sr_number,post_office from post_master where district_id=" + postOfficeRequest.districtId + " and  status_id = 1 order by post_office";
                var postOfficeList = DapperHelper.GetRecords<PostOfficeProperties>(query,SQLMode.Query,null);
                if (postOfficeList.Count > 0)
                {
                    postOfficeResponse.postOfficeList = postOfficeList;
                    postOfficeResponse.status.code = APIStatus.success;
                    postOfficeResponse.status.message = "Success";
                    postOfficeResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    postOfficeResponse.status.code = APIStatus.no_Data_Found;
                    postOfficeResponse.status.message = "No Data Found";
                    postOfficeResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                postOfficeResponse.status.code = APIStatus.exception;
                postOfficeResponse.status.message = ex.Message;
                postOfficeResponse.status.flag = ProcessStatus.failed;

            }

            return postOfficeResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get PreNameList</summary>
        /// <returns><see cref="PreNameResponse"/></returns>
        public PreNameResponse getPreNameList()
        {
          //DBAccessHelper helper = new DBAccessHelper();
            PreNameResponse prenameResponse = new PreNameResponse();

            try
            {
                var query = "select PRENAME_ID,PRENAME,GENDER from prename_master order by PRENAME";
                var preNameList = DapperHelper.GetRecords<PreNameProperties>(query,SQLMode.Query,null);
                if (preNameList.Count > 0)
                {
                    prenameResponse.preNameList = preNameList;
                    prenameResponse.status.code = APIStatus.success;
                    prenameResponse.status.message = "Success";
                    prenameResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    prenameResponse.status.code = APIStatus.no_Data_Found;
                    prenameResponse.status.message = "No Data Found";
                    prenameResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                prenameResponse.status.flag = ProcessStatus.failed;
                prenameResponse.status.code = APIStatus.exception;
                prenameResponse.status.message = ex.Message;
            }
            return prenameResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get States details</summary>
        /// <param name="StatesRequest"><see cref="StatesRequest"/></param>
        /// <returns><see cref="StatesResponset"/></returns>
        public StatesResponset getStates(StatesRequest statesRequest)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            StatesResponset statesResponset = new StatesResponset();

            try
            {
                var query = "select state_id,state_name from state_master where country_id =" + statesRequest.countryId + "  order by state_name";
                var statesList = DapperHelper.GetRecords<StatesProperties>(query,SQLMode.Query,null);
                if (statesList.Count > 0)
                {
                    statesResponset.statesList = statesList;
                    statesResponset.status.code = APIStatus.success;
                    statesResponset.status.message = "Success";
                    statesResponset.status.flag = ProcessStatus.success;
                }
                else
                {
                    statesResponset.status.code = APIStatus.no_Data_Found;
                    statesResponset.status.message = "No Data Found";
                    statesResponset.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                statesResponset.status.code = APIStatus.exception;
                statesResponset.status.message = ex.Message;
                statesResponset.status.flag = ProcessStatus.failed;

            }

            return statesResponset;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get StatusMaster</summary>
        /// <param name="StatusRequest"><see cref="StatusRequest"/></param>
        /// <returns><see cref="StatusResponse"/></returns>
        public StatusResponse getStatusMaster(StatusRequest statusRequest)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            StatusResponse statusResponse = new StatusResponse();

            try
            {
                var query = "select t.status_id, t.description from STATUS_MASTER t where t.module_id = " + statusRequest.moduleId + " and t.option_id = " + statusRequest.optionId + " and t.description is not null order by t.description";
                var statusList = DapperHelper.GetRecords<StatusProperties>(query,SQLMode.Query,null);
                if (statusList.Count > 0)
                {
                    statusResponse.statusList = statusList;
                    statusResponse.status.code = APIStatus.success;
                    statusResponse.status.message = "Success";
                    statusResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    statusResponse.status.code = APIStatus.no_Data_Found;
                    statusResponse.status.message = "No Data Found";
                    statusResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                statusResponse.status.flag = ProcessStatus.failed;
                statusResponse.status.code = APIStatus.exception;
                statusResponse.status.message = ex.Message;
            }
            return statusResponse;
        }


        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get pin serial details</summary>
        /// <param name="pinCode">pinCode</param>
        /// <returns><see cref="DetailsResponse"/></returns>
        public DetailsResponse getpinserialDetails(long pinCode)
        {
          //DBAccessHelper helper = new DBAccessHelper();
            DetailsResponse detailsResponse = new DetailsResponse();

            try
            {
               

                    var query = "select pm.pin_code, to_char(pm.sr_number) as postofficeId, pm.post_office, to_char(dm.district_id) as districtId, " +
                                " dm.district_name,to_char(sm.state_id) as stateId, sm.state_name, to_char(cd.country_id) as countryId,cd.country_name " +
                                " from post_master pm, district_master dm, state_master sm, country_dtl cd where pm.sr_number = " + pinCode + "" +
                                " and dm.district_id = pm.district_id and sm.state_id = dm.state_id and cd.country_id = sm.country_id  order by pm.post_office ";

                    var details = DapperHelper.GetRecords<DetailsProperties>(query,SQLMode.Query,null);
                    if (details.Count > 0)
                    {
                        detailsResponse.detailsList = details;
                        detailsResponse.status.code = APIStatus.success;
                        detailsResponse.status.message = "Success";
                        detailsResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        detailsResponse.status.code = APIStatus.no_Data_Found;
                        detailsResponse.status.message = "No Data Found";
                        detailsResponse.status.flag = ProcessStatus.success;
                    }
             

            }
            catch (Exception ex)
            {
                detailsResponse.status.code = APIStatus.exception;
                detailsResponse.status.message = ex.Message;
                detailsResponse.status.flag = ProcessStatus.failed;

            }

            return detailsResponse;
        }

        //--- ocuupation subcategory---done by  Sreerekha K ----7-Apr-2020 --CRF- Risk Categorization of Customers

        public OccupationSubCategoryResponse getOccupationSubCategory(OccupationSubCategoryRequest  Request)
        {        
            OccupationSubCategoryResponse  Response = new OccupationSubCategoryResponse();
            try
            {
                var query = "select l.sub_category_id, l.sub_cat_name from TBL_OCCUPATION_SUB_CATEGORY l where l.cat_id = " + Request.CategoryId + " and l.status_id = 1";
                var OccupationSubCategoryList = DapperHelper.GetRecords<OccupationProperties>(query, SQLMode.Query, null);
                var query1 = "select l.category_id, l.category_name from TBL_OCCUPATION_CATEGORY l where l.master_id =  " + Request.OccupationId + "  and l.status = 1";
                var CategoryList = DapperHelper.GetRecords<OccupationCategoryProperties>(query1, SQLMode.Query, null);
                if (OccupationSubCategoryList.Count > 0)
                {
                     Response.occupationSubCategoryList = OccupationSubCategoryList;
                     Response.status.code = APIStatus.success;
                     Response.status.message = "Success";
                     Response.status.flag = ProcessStatus.success;
                }
                if (CategoryList.Count>0)
                {
                    Response.occupationCategoryList = CategoryList;
                    Response.status.code = APIStatus.success;
                    Response.status.message = "Success";
                    Response.status.flag = ProcessStatus.success;
                }
                if (CategoryList.Count == 0  && OccupationSubCategoryList.Count==0)
                {
                    Response.status.code = APIStatus.no_Data_Found;
                    Response.status.message = "No Data Found";
                    Response.status.flag = ProcessStatus.success;
                }


            }
            catch (Exception ex)
            {
                Response.status.code = APIStatus.exception;
                Response.status.message = ex.Message;
                Response.status.flag = ProcessStatus.failed;

            }

            return Response;
        }

    }
}
