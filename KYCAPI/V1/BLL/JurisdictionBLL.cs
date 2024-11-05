using System;
using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GlobalValues;
using KYCAPI.V1.BLLDependency;
using KYCAPI.V1.Models.Response;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.BLL
{
    public class JurisdictionBLL : APIBaseBLL , IJurisdictionBLL
    {

        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public JurisdictionBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }

        public JurisdictionResponse getJurisdictionDetails()
        {
            JurisdictionResponse response = new JurisdictionResponse();
            try
            {
             //   DBAccessHelper helper = new DBAccessHelper();
                string query = "";
                JurisdictionResponse consent = DapperHelper.GetRecord<JurisdictionResponse>(query,SQLMode.Query,null);
                if (consent!=null)
                {
                    response =consent;
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.status.code = APIStatus.no_Data_Found;
                    response.status.message = "No data found for your search";
                    response.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception e)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = e.Message;
            }

            return response;
        }



    }
}
