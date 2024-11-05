using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccessLibrary
{
    public interface IDBAccessHelper
    {
        int ExecuteNonQuery(string query);
        int ExecuteNonQuery(string query, OracleParameter[] parameters_value);
        T ExecuteScalar<T>(string query);
        object ExecuteScalar(string query, OracleParameter[] parameters_value);
        OracleDataReader ExecuteReader(string query);
        OracleDataReader ExecuteReader(string query, OracleParameter[] parameters_value);
        DataSet ExecuteDataSet(string query);
        DataSet ExecuteDataSet(string query, OracleParameter[] parameters_value);
        DataSet ExecuteMDataSet(string[] query, string[] tables);
    }
}
