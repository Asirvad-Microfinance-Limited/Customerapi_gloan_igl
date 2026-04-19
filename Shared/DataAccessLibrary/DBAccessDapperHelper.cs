using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace DataAccessLibrary
{
    public class DBAccessDapperHelper
    {
        
        public enum SQLMode
        {
            Query = 1,
            StoredProcedure = 2
        }

        private string strConnectionString = "";
        AppConfiguration appConfiguration = new AppConfiguration();
        PasswordSecurity passwordSecurity = new PasswordSecurity();
        public DBAccessDapperHelper()
        {
            var con = appConfiguration.ConnectionString;

            var pwd = appConfiguration.GetPassword;
            strConnectionString = con + passwordSecurity.Decrypt(pwd);


            // strConnectionString = "Data Source=(DESCRIPTION= (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.192.5.13 )(PORT=1521))) (CONNECT_DATA=(SERVICE_NAME=testdb))); User Id=mana0809 ;Password=asd#1234";
            //  strConnectionString = "data source=BACKUPDB;password=asd#1234;persist security info=True;user id=cgl";
        }

        public int ExecuteNonQuery(string query, SQLMode sqlMode, DynamicParameters dyParam)
        {
            int retVal = -1;
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                OracleParameter oracleParameter = new OracleParameter();
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    retVal = SqlMapper.Execute(objConnection, query, dyParam, commandType: commandType);
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return retVal;

        }
        public SqlMapper.GridReader ExecuteMultipleQuery(string query, SQLMode sqlMode, OracleDynamicParameters dyParam)
        {
            SqlMapper.GridReader retVal;
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                OracleParameter oracleParameter = new OracleParameter();
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    retVal = SqlMapper.QueryMultiple(objConnection, query, dyParam, commandType: commandType);
                    //objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                //objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return retVal;

        }


        public int ExecuteNonQuerywithOracleDynamicParameters(string query, SQLMode sqlMode, OracleDynamicParameters dyParam)
        {
            int retVal = -1;
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                OracleParameter oracleParameter = new OracleParameter();
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    retVal = SqlMapper.Execute(objConnection, query, dyParam, commandType: commandType);
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return retVal;

        }


        public DataTable ExecuteReaderwithOracleDynamicParameters(string query, SQLMode sqlMode, OracleDynamicParameters dyParam)
        {
           
            var dataTable = new DataTable();
           
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                OracleParameter oracleParameter = new OracleParameter();
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    var dataReader = SqlMapper.ExecuteReader(objConnection, query, dyParam, commandType: commandType);
                    dataTable.Load(dataReader);
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return dataTable;

        }


        public T ExecuteScalar<T>(string query, SQLMode sqlMode, DynamicParameters dyParam)
        {
            T Obj = Activator.CreateInstance<T>();
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    Obj = SqlMapper.ExecuteScalar<T>(objConnection, query, dyParam, commandType: commandType);
                    objConnection.Close();
                }
            }catch(Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return Obj;
        }

        public T GetRecord<T>(string queryName, SQLMode sqlMode, DynamicParameters dyParam)
        {
            T objRecord = default(T);
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    objRecord = SqlMapper.Query<T>(objConnection,queryName, dyParam, commandType: commandType).FirstOrDefault();
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return objRecord;
        }
        public T GetRecordwithOracleDynamicParameters<T>(string queryName, SQLMode sqlMode, OracleDynamicParameters dyParam)
        {
            T objRecord = default(T);
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    objRecord = SqlMapper.Query<T>(objConnection, queryName, dyParam, commandType: commandType).FirstOrDefault();
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return objRecord;
        }
        public List<T> GetRecords<T>(string queryName, SQLMode sqlMode, DynamicParameters dyParam)
        {
            List<T> recordList = new List<T>();
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    recordList = SqlMapper.Query<T>(objConnection, queryName, dyParam, commandType: commandType).ToList();
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return recordList;
        }

        public List<T> GetRecordswithOracleDynamicParameters<T>(string queryName, SQLMode sqlMode, OracleDynamicParameters dyParam)
        {
            List<T> recordList = new List<T>();
            OracleConnection objConnection = new OracleConnection();
            try
            {
                CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
                using (objConnection = new OracleConnection(strConnectionString))
                {
                    objConnection.Open();
                    recordList = SqlMapper.Query<T>(objConnection, queryName, dyParam, commandType: commandType).ToList();
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objConnection.Close();
                throw ex;
            }
            finally
            {
                if ((objConnection.State == ConnectionState.Open))
                    objConnection.Close();
                objConnection.Dispose();
            }
            return recordList;
        }


    }
}
