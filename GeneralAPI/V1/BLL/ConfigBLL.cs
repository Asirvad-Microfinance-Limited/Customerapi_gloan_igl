using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using GlobalValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.BLL
{
    public class ConfigBLL : APIBaseBLL,IConfigBLL
    {
        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public ConfigBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }
        public ConfigInsertResponse insertConfig(ConfigInsertRequest request)
        {
           
            ConfigInsertResponse response = new ConfigInsertResponse();
            try
            {
                string query = "insert into tbl_gl_api_config t(t.config_id,t.config_name,config_status,t.config_date) values(config_sequence.NEXTVAL,'" + request.configName + "',1,sysdate)";
                int checkret = helper.ExecuteNonQuery(query);
                
                    if (checkret == 1)
                    {
                        
                        response.status.code = APIStatus.success;
                        response.status.message = "Success";
                        response.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        response.status.code = APIStatus.failed;
                        response.status.message = "failed";
                        response.status.flag = ProcessStatus.success;
                    }
            }
            catch(Exception ex)
            {

                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
                response.status.flag = ProcessStatus.failed;
            }

            return response;

        }

        public ConfigUpdateResponse updateConfig(ConfigUpdateRequest request)
        {
            ConfigUpdateResponse response = new ConfigUpdateResponse();
            try
            {
               
                string query = "update tbl_gl_api_config t set t.config_name = '"+ request.configName + "', t.config_status = " + request.configStatus + " , t.config_date = sysdate where t.config_id = " + request.configId + "";
                int checkret = helper.ExecuteNonQuery(query);
                if (checkret == 1)
                    if (checkret == 1)
                    {

                        response.status.code = APIStatus.success;
                        response.status.message = "Success";
                        response.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        response.status.code = APIStatus.failed;
                        response.status.message = "failed";
                        response.status.flag = ProcessStatus.success;
                    }


            }
            catch (Exception ex)
            {

                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
                response.status.flag = ProcessStatus.failed;
            }

            return response;

        }

        public ConfigDetailsResponse getConfig()
        {
            ConfigDetailsResponse response = new ConfigDetailsResponse();
            try
            {

                string query = "select t.config_id, t.config_name,t.config_status from tbl_gl_api_config t";
                List<ConfigDetailsProperties> configDetails = DapperHelper.GetRecords<ConfigDetailsProperties>(query,SQLMode.Query,null);
                if (configDetails.Count > 0)
                {
                    response.configDetails = configDetails;
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.status.code = APIStatus.no_Data_Found;
                    response.status.message = "No Data Found";
                    response.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {

                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
                response.status.flag = ProcessStatus.failed;
            }

            return response;

        }
    }
}
