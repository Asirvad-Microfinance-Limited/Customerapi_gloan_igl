using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccessLibrary
{
    public class DBAccessHelper
    {
        private string strConnectionString = "";
        AppConfiguration appConfiguration = new AppConfiguration();
        PasswordSecurity passwordSecurity = new PasswordSecurity();
        public DBAccessHelper()
        {
            var con = appConfiguration.ConnectionString;

            var pwd = appConfiguration.GetPassword;
            strConnectionString = con + passwordSecurity.Decrypt(pwd);


           // strConnectionString = "Data Source=(DESCRIPTION= (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.192.5.13 )(PORT=1521))) (CONNECT_DATA=(SERVICE_NAME=testdb))); User Id=mana0809 ;Password=asd#1234";
          //  strConnectionString = "data source=BACKUPDB;password=asd#1234;persist security info=True;user id=cgl";
        }

        public int ExecuteNonQuery(string query)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            OracleCommand cmd = new OracleCommand(query, cnn);
            if ((query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete") | query.StartsWith("exec")))
                cmd.CommandType = CommandType.Text;
            else
                cmd.CommandType = CommandType.StoredProcedure;
            int retval;
            try
            {
                cnn.Open();
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            // ExceptionHelper.ExceptionHelper exphelper = new ExceptionHelper.ExceptionHelper();
            // exphelper.PublishInDatabase(exp);
            // exphelper.PublishInEventLog(exp);
            // exphelper.PublishInEmail(exp);
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cnn.Dispose();
                cmd.Dispose();
            }
            return retval;
        }

        public int ExecuteNonQuery(string query, OracleParameter[] parameters_value)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            OracleCommand cmd = new OracleCommand(query, cnn);
            int retVal = -1;
            try
            {
                if ((query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete")))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;
                int i;
                for (i = 0; i <= parameters_value.Length - 1; i++)
                    cmd.Parameters.Add(parameters_value[i]);
                cnn.Open();
                retVal = cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {
            }

            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
            return retVal;
        }
        public T ExecuteScalar<T>(string query)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            OracleCommand cmd = new OracleCommand(query, cnn);
            T retval = Activator.CreateInstance<T>();
            try
            {
                if ((query.StartsWith("SELECT") | query.StartsWith("select")))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;
                cnn.Open();
                retval = (T)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
            return retval;
        }
        public object ExecuteScalar(string query, OracleParameter[] parameters_value)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            object retval = new object();
            OracleCommand cmd = new OracleCommand(query, cnn);
            try
            {
                if ((query.StartsWith("SELECT") | query.StartsWith("select")))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;
                int i;
                for (i = 0; i <= parameters_value.Length - 1; i++)
                    cmd.Parameters.Add(parameters_value[i]);
                cnn.Open();
                retval = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
            return retval;
        }
        public OracleDataReader ExecuteReader(string query)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            OracleCommand cmd = new OracleCommand(query, cnn);
            OracleDataReader retval = null;
            try
            {
                if ((query.StartsWith("SELECT") | query.StartsWith("select")))
                {
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    retval = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cnn.Open();
                    retval = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
            return retval;
        }
        public OracleDataReader ExecuteReader(string query, OracleParameter[] parameters_value)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            OracleCommand cmd = new OracleCommand(query, cnn);
            OracleDataReader retval = null;
            try
            {
                if ((query.StartsWith("SELECT") | query.StartsWith("select")))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;
                int i;
                for (i = 0; i <= parameters_value.Length - 1; i++)
                    cmd.Parameters.Add(parameters_value[i]);
                cnn.Open();
                retval = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
            return retval;
        }
        public DataSet ExecuteDataSet(string query)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            var cmd = new OracleCommand(query, cnn);
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            try
            {
                if ((query.ToUpper().StartsWith("SELECT") | query.ToLower().StartsWith("select")))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                da.SelectCommand = cmd;

                da.Fill(ds);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cmd.Dispose();
                cnn.Dispose();
                da.Dispose();
            }
            return ds;
        }
        public DataSet ExecuteDataSet(string query, OracleParameter[] parameters_value)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            var cmd = new OracleCommand(query, cnn);
            OracleDataAdapter da = new OracleDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                if ((query.StartsWith("SELECT") | query.StartsWith("select")))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;
                int i;
                for (i = 0; i <= parameters_value.Length - 1; i++)
                    cmd.Parameters.Add(parameters_value[i]);

                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cmd.Dispose();
                cnn.Dispose();
                da.Dispose();
            }
            return ds;
        }
        public DataSet ExecuteMDataSet(string[] query, string[] tables)
        {
            OracleConnection cnn = new OracleConnection(strConnectionString);
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            try
            {
                int i;
                for (i = 0; i <= query.GetUpperBound(0); i++)
                {
                    var cmd = new OracleCommand(query[i], cnn);
                    cmd.CommandType = CommandType.Text;
                    da.SelectCommand = cmd;
                    da.Fill(ds, tables[i]);
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((cnn.State == ConnectionState.Open))
                    cnn.Close();
                cnn.Dispose();
                da.Dispose();
            }
            return ds;
        }
        public void dispose()
        {
            this.dispose();
        }

    }
}
