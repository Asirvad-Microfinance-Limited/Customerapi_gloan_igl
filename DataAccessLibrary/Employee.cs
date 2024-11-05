using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataAccessLibrary
{
    public class Employees:IEmployee
    {
        IDBAccessHelper oh;
        public Employees(IDBAccessHelper _oh)
        {
            oh = _oh;
        }

        public bool CheckUser(int userName, string password)
        {
            byte[] hashedDataBytes;
            //DBAccessHelper oh = new DBAccessHelper();
            hashedDataBytes = getEdata(userName.ToString(), password);
          
            string sql = "select emp_code from employee_master where emp_code=" + userName + " and password=:psd";
           
            OracleParameter[] parm_coll = new OracleParameter[1];
            parm_coll[0] = new OracleParameter("psd", OracleDbType.Raw);
            parm_coll[0].Value = hashedDataBytes;
            parm_coll[0].Direction = ParameterDirection.Input;

            System.Data.DataTable dt;
            dt = oh.ExecuteDataSet(sql, parm_coll).Tables[0];
            if (dt.Rows.Count == 0)
                return false;
            else
                return true;
        }
        public bool CheckUser(int userName, string password, ref int access_id, ref int role_id, ref int emp_branch, ref int passwd_flag)
        {
            byte[] hashedDataBytes;
            hashedDataBytes = getEdata(userName.ToString(), password);
           // DBAccessHelper oh = new DBAccessHelper();
            OracleParameter[] parm_coll = new OracleParameter[6];

            parm_coll[0] = new OracleParameter("empid", OracleDbType.Long,6);
            parm_coll[0].Value = userName;
            parm_coll[0].Direction = ParameterDirection.Input;

            parm_coll[1] = new OracleParameter("accessid", OracleDbType.Long,5);
            parm_coll[1].Value = null;
            parm_coll[1].Direction = ParameterDirection.Output;

            parm_coll[2] = new OracleParameter("roleid", OracleDbType.Long,5);
            parm_coll[2].Direction = ParameterDirection.Output;

            parm_coll[3] = new OracleParameter("emp_br", OracleDbType.Long,5);
            parm_coll[3].Direction = ParameterDirection.Output;


            parm_coll[4] = new OracleParameter("passwd", OracleDbType.Raw,16);
            parm_coll[4].Value = hashedDataBytes;
            parm_coll[4].Direction = ParameterDirection.Input;

            parm_coll[5] = new OracleParameter("passwd_flg", OracleDbType.Long,5);
            parm_coll[5].Direction = ParameterDirection.Output;

            oh.ExecuteNonQuery("get_access_level", parm_coll);

            //p[0] = new OracleClient.OracleParameter("empid", OracleClient.OracleType.Number, 6);
            //p[0].Value = userName;
            //p[0].Direction = ParameterDirection.Input;
            //p[1] = new OracleClient.OracleParameter("accessid", OracleClient.OracleType.Number, 5);
            //p[1].Direction = ParameterDirection.Output;
            //p[2] = new OracleClient.OracleParameter("roleid", OracleClient.OracleType.Number, 5);
            //p[2].Direction = ParameterDirection.Output;
            //p[3] = new OracleClient.OracleParameter("emp_br", OracleClient.OracleType.Number, 5);
            //p[3].Direction = ParameterDirection.Output;
            //p[4] = new OracleClient.OracleParameter("passwd", OracleClient.OracleType.Raw, 16);
            //p[4].Direction = ParameterDirection.Input;
            //p[4].Value = hashedDataBytes;
            //p[5] = new OracleClient.OracleParameter("passwd_flg", OracleClient.OracleType.Number, 5);
            //p[5].Direction = ParameterDirection.Output;
            //oh.ExecuteNonQuery("get_access_level", p);
            access_id = Convert.ToInt32( parm_coll[1].Value);
            role_id = Convert.ToInt32(parm_coll[2].Value);
            emp_branch = Convert.ToInt32(parm_coll[3].Value);
            passwd_flag = Convert.ToInt32(parm_coll[5].Value);
            return true;
        }
        public byte[] getEdata(string userName, string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(userName + "raju" + password));
            return hashedDataBytes;
        }

        public string jsDecrypt(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
