using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccessLibrary
{
    public interface IDBAccessDapperHelper
    {
        int ExecuteNonQuery(string query, SQLMode sqlMode, DynamicParameters dyParam);
        SqlMapper.GridReader ExecuteMultipleQuery(string query, SQLMode sqlMode, OracleDynamicParameters dyParam);
        int ExecuteNonQuerywithOracleDynamicParameters(string query, SQLMode sqlMode, OracleDynamicParameters dyParam);
        DataTable ExecuteReaderwithOracleDynamicParameters(string query, SQLMode sqlMode, OracleDynamicParameters dyParam);
        T ExecuteScalar<T>(string query, SQLMode sqlMode, DynamicParameters dyParam);
        T GetRecord<T>(string queryName, SQLMode sqlMode, DynamicParameters dyParam);
        T GetRecordwithOracleDynamicParameters<T>(string queryName, SQLMode sqlMode, OracleDynamicParameters dyParam);
        List<T> GetRecords<T>(string queryName, SQLMode sqlMode, DynamicParameters dyParam);
        List<T> GetRecordswithOracleDynamicParameters<T>(string queryName, SQLMode sqlMode, OracleDynamicParameters dyParam);
    }

    public enum SQLMode
    {
        Query = 1,
        StoredProcedure = 2
    }
}
