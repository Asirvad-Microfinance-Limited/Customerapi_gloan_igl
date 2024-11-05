using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccessLibrary
{
    public class PHelper
    {
        public string getRoles(long userId, string passWord)
        {
            DBAccessDapperHelper helper = new DBAccessDapperHelper();
            string result = string.Empty;
            try
            {
                DynamicParameters parm_coll = new DynamicParameters();

                parm_coll.Add("empid", userId, DbType.Int64, ParameterDirection.Input);
                parm_coll.Add("passwd", passWord, DbType.String, ParameterDirection.Input);
                parm_coll.Add("accessid", 0, DbType.Int64, ParameterDirection.Output);
                parm_coll.Add("roleid", 0, DbType.Int64, ParameterDirection.Output);
                parm_coll.Add("emp_br", 0, DbType.Int64, ParameterDirection.Output);
                parm_coll.Add("passwd_flg", 0, DbType.Int64, ParameterDirection.Output);

                helper.ExecuteNonQuery("get_roles", SQLMode.StoredProcedure, parm_coll);

                decimal accessid = parm_coll.Get<decimal>("accessid");
                decimal roleid = parm_coll.Get<decimal>("roleid");
                decimal emp_br = parm_coll.Get<decimal>("emp_br");
                decimal passwd_flg = parm_coll.Get<decimal>("passwd_flg");

                result = accessid.ToString() + "-" + passwd_flg.ToString() + "-" + emp_br.ToString() + "-" + roleid.ToString();

            }
            catch (Exception ex)
            {

                result = "Error";

            }

            return result;
        }

        public long password_chek(long usid, string passwd)
        {
            DBAccessDapperHelper helper = new DBAccessDapperHelper();
            long logdat = 0;
            try
            {
                string query = "select count(*)   from employee_master where emp_code=" + usid + " and password='" + passwd + "' and status_id=1";
                object value = helper.ExecuteScalar<object>(query, SQLMode.Query, null);
                logdat = Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                logdat = 0;
            }
            return logdat;
        }

        public string change_password(long userID, string oldPasswd, string newpass)
        {
            DBAccessDapperHelper helper = new DBAccessDapperHelper();
            string result = string.Empty;
            try
            {
                DynamicParameters parm_coll = new DynamicParameters();

                parm_coll.Add("user_nm", userID, DbType.Int64, ParameterDirection.Input);
                parm_coll.Add("oldpass", oldPasswd, DbType.String, ParameterDirection.Input);
                parm_coll.Add("newpass", newpass, DbType.String, ParameterDirection.Input);
                parm_coll.Add("msg", string.Empty, DbType.String, ParameterDirection.Output);

                helper.ExecuteNonQuery("change_password_new", SQLMode.StoredProcedure, parm_coll);

                string msg = parm_coll.Get<string>("msg");
                result = msg;
            }
            catch (Exception ex)
            {

                result = "";

            }

            return result;
        }

        public long login_track(long userid, int res, string sys_ip, long app_id)
        {
            DBAccessDapperHelper helper = new DBAccessDapperHelper();
            long m = 0;
            try
            {
                DynamicParameters parm_coll = new DynamicParameters();

                parm_coll.Add("emcod", userid, DbType.Int64, ParameterDirection.Input);
                parm_coll.Add("stat", res, DbType.Int64, ParameterDirection.Output);
                parm_coll.Add("sys_ip", sys_ip, DbType.String, ParameterDirection.Input);
                parm_coll.Add("modid", app_id, DbType.String, ParameterDirection.Input);

                helper.ExecuteNonQuery("pro_hrm_log_track", SQLMode.StoredProcedure, parm_coll);

                long stat = parm_coll.Get<long>("stat");
                m = stat;
            }
            catch (Exception ex)
            {

                m = 0;

            }

            return m;
        }
    }
}
