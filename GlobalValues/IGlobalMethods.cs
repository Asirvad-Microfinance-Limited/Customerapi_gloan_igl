using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace GlobalValues
{
    public interface IGlobalMethods
    {
        List<T> ConvertDataTable<T>(DataTable dt);
        T ConvertClass<T>(DataTable dt);
        T GetItem<T>(DataRow dr);
        List<T> GetList<T>(string query);
        object GetClass<T>(string query);
        T GetClassNew<T>(string query);
        T InvokePostHttpClient<T, F>(F obj, string url);
        T InvokePostHttpClientWithoutRequest<T>(string url);
        Task<T> InvokePostHttpClientasyc<T, F>(F obj, string url, string authToken = null);
        string BuildACEOnPremiseRequestUrl(string ip, string port, string url = "");
        T InvokeGetHttpClientWithoutRequest<T>(string url, string authToken);
    }
}
