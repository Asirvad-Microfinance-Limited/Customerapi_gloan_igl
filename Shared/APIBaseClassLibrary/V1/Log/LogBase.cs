using System;
using System.Data;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Models.Request;
using APIBaseClassLibrary.V1.Models.Response;
using DataAccessLibrary;
using GlobalLibrary;
using Oracle.ManagedDataAccess.Client;
using static GlobalLibrary.GlobalVariables;

namespace APIBaseClassLibrary.V1.Log
{
    public class LogBase
    {

        //IConfiguration configuration;
        //IGlobalMethods globalMethods;
        //public LogBase(IConfiguration iConfig, IGlobalMethods _globalMethods)
        //{
        //    configuration = iConfig;
        //    globalMethods = _globalMethods;
        //}


        public async Task<GlAuditLogResponse> addAuditLog(GlAuditLogRequest request)
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            GlAuditLogResponse response = new GlAuditLogResponse();
            try
            {
                string baseUrl = appConfiguration.ServiceUrl;
                string generalUrl = $"/logapi/api/v1/log/addauditrequestlog";
                string Url = baseUrl + generalUrl;
                response = await GlobalMethods.InvokePostHttpClientasyc<GlAuditLogResponse, GlAuditLogRequest>(request, Url);
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }

            return await Task.FromResult<GlAuditLogResponse>(response);
        }

        public async Task<GlErrorLogResponse> addErrorLog(GlErrorLogRequest request)
        {
            GlErrorLogResponse response = new GlErrorLogResponse();
            AppConfiguration appConfiguration = new AppConfiguration();
            try
            {
                string baseUrl = appConfiguration.ServiceUrl;
                string generalUrl = $"/logapi/api/v1/log/adderrorlog";
                string Url = baseUrl + generalUrl;
                response = await GlobalMethods.InvokePostHttpClientasyc<GlErrorLogResponse, GlErrorLogRequest>(request, Url);
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }

            return await Task.FromResult<GlErrorLogResponse>(response);
        }

        public async Task<GlAuditLogResponse> updateAuditLog(GlAuditLogUpdateRequest request)
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            GlAuditLogResponse response = new GlAuditLogResponse();
            try
            {
                string baseUrl = appConfiguration.ServiceUrl;
                string generalUrl = $"/logapi/api/v1/log/addauditresponselog";
                string Url = baseUrl + generalUrl;
                response = await GlobalMethods.InvokePostHttpClientasyc<GlAuditLogResponse, GlAuditLogUpdateRequest>(request, Url);
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }

            return await Task.FromResult<GlAuditLogResponse>(response);
        }

        //public async Task<GlAuditLogResponse> addAuditLog(GlAuditLogRequest request)
        //{
        //    GlAuditLogResponse response = new GlAuditLogResponse();
        //    DBAccessHelper helper = new DBAccessHelper();
        //    try
        //    {
        //        string str1 = "insert into TBL_AUDIT_LOG (log_id,branch_id,emp_code,url,method,controller_name,action_name,log_date,request,request_time ) values ('" + request.LOG_ID + "'," + request.BRANCH_ID + "," + request.EMP_CODE + ",'" + request.URL + "','" + request.METHOD + "','" + request.CONTROLLER_NAME + "','" + request.ACTION_NAME + "',to_date(sysdate), :request, '" + request.REQUEST_TIME + "') ";
        //        OracleParameter[] parm1 = new OracleParameter[1];
        //        parm1[0] = new OracleParameter();
        //        parm1[0].ParameterName = "request";
        //        parm1[0].OracleDbType = OracleDbType.Clob;
        //        parm1[0].Direction = ParameterDirection.Input;
        //        parm1[0].Value = request.REQUEST;
        //        if (helper.ExecuteNonQuery(str1, parm1) == 1)
        //        {
        //            response.status.code = APIStatus.success;
        //            response.status.message = "Success";
        //            response.status.flag = ProcessStatus.success;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.status.flag = ProcessStatus.failed;
        //        response.status.code = APIStatus.exception;
        //        response.status.message = ex.Message;
        //    }

        //    return await Task.FromResult<GlAuditLogResponse>(response);
        //}

        //public async Task<GlErrorLogResponse> addErrorLog(GlErrorLogRequest request)
        //{
        //    GlErrorLogResponse response = new GlErrorLogResponse();
        //    DBAccessHelper helper = new DBAccessHelper();
        //    try
        //    {
        //        string str1 = "insert into TBL_ERROR_LOG (log_id,branch_id,emp_code,url,method,controller_name,action_name,exception,inner_exception,LOG_DATE,STACKTRACE) values ('" + request.LOG_ID + "'," + request.BRANCH_ID + "," + request.EMP_CODE + ",'" + request.URL + "','" + request.METHOD + "','" + request.CONTROLLER_NAME + "','" + request.ACTION_NAME + "','" + request.EXCEPTION + "','" + request.INNER_EXCEPTION + "', sysdate,:STACKTRACE) ";
        //        OracleParameter[] parm1 = new OracleParameter[1];
        //        parm1[0] = new OracleParameter();
        //        parm1[0].ParameterName = "STACKTRACE";
        //        parm1[0].OracleDbType = OracleDbType.Clob;
        //        parm1[0].Direction = ParameterDirection.Input;
        //        parm1[0].Value = request.STACKTRACE;
        //        if (helper.ExecuteNonQuery(str1, parm1) == 1)
        //        {
        //            response.status.code = APIStatus.success;
        //            response.status.message = "Success";
        //            response.status.flag = ProcessStatus.success;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.status.flag = ProcessStatus.failed;
        //        response.status.code = APIStatus.exception;
        //        response.status.message = ex.Message;
        //    }

        //    return await Task.FromResult<GlErrorLogResponse>(response);
        //}

        //public async Task<GlAuditLogResponse> updateAuditLog(GlAuditLogUpdateRequest request)
        //{
        //    GlAuditLogResponse response = new GlAuditLogResponse();
        //    DBAccessHelper helper = new DBAccessHelper();
        //    try
        //    {
        //        string str1 = "update  TBL_AUDIT_LOG set RESPONSE = :RESPONSE , RESPONSE_TIME = '" + request.RESPONSE_TIME + "' where  LOG_ID = '" + request.LOG_ID + "'";
        //        OracleParameter[] parm1 = new OracleParameter[1];
        //        parm1[0] = new OracleParameter();
        //        parm1[0].ParameterName = "RESPONSE";
        //        parm1[0].OracleDbType = OracleDbType.Clob;
        //        parm1[0].Direction = ParameterDirection.Input;
        //        parm1[0].Value = request.RESPONSE;

        //        if (helper.ExecuteNonQuery(str1, parm1) == 1)
        //        {
        //            response.status.code = APIStatus.success;
        //            response.status.message = "Success";
        //            response.status.flag = ProcessStatus.success;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.status.flag = ProcessStatus.failed;
        //        response.status.code = APIStatus.exception;
        //        response.status.message = ex.Message;
        //    }

        //    return await Task.FromResult<GlAuditLogResponse>(response);
        //}
    }
}
