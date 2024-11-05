using DataAccessLibrary;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GlobalValues
{
    public  class GlobalMethods: IGlobalMethods
    {




        public  List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public  T ConvertClass<T>(DataTable dt)
        {
            T item = default(T);
            foreach (DataRow row in dt.Rows)
            {
                item = GetItem<T>(row);

            }
            return item;
        }

        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == column.ColumnName.ToLower())
                    {
                        pro.SetValue(obj, (dr[column.ColumnName] == DBNull.Value) ? null : dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }



        public  List<T> GetList<T>(string query)
        {
            List<T> customerTypes = new List<T>();
            DBAccessHelper helper = new DBAccessHelper();
            DataSet ds = helper.ExecuteDataSet(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {

                    customerTypes = ConvertDataTable<T>(dt);

                }

            }

            return customerTypes;
        }

        public  object GetClass<T>(string query)
        {
            object obj =null;
            DBAccessHelper helper = new DBAccessHelper();
            DataSet ds = helper.ExecuteDataSet(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {

                    obj = ConvertClass<T>(dt);

                }

            }

            return obj;
        }

        public  T GetClassNew<T>(string query)
        {
          
            T Obj = Activator.CreateInstance<T>();
            DBAccessHelper helper = new DBAccessHelper();
            DataSet ds = helper.ExecuteDataSet(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {

                    Obj = ConvertClass<T>(dt);

                }

            }

            return Obj;
        }

        public  T InvokePostHttpClient<T, F>(F obj, string url)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                response = httpClient.PostAsync(url, new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            }

            return (T)ob;
        }
        public async  Task<T> InvokePostHttpClientasyc<T, F>(F obj, string url, string authToken = null)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            using (var httpClient = new HttpClient())
            {
                if (authToken != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);
                }
                response = httpClient.PostAsync(url, new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            }
            return await Task.FromResult<T>((T)ob);
        }


        public T InvokePostHttpClientWithoutRequest<T>(string url)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };

            var jsonResponse = string.Empty;
            Object ob = new object();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                response = httpClient.PostAsync(url, null).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            }

            return (T)ob;
        }

        public string BuildACEOnPremiseRequestUrl(string ip, string port, string url = "")
        {
            string urlFormat = "http://{0}:{1}";
            string requestUrl = "";
            if (!string.IsNullOrEmpty(url))
            {
                requestUrl = url;
            }
            else if (!string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
            {
                requestUrl = ip;
            }
            else
            {
                requestUrl = string.Format(urlFormat, ip, port);
            }
            return requestUrl;
        }

        
        public  T InvokeGetHttpClientWithoutRequest<T>(string url,string authToken)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };

            var jsonResponse = string.Empty;
            Object ob = new object();
            using (var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            {
                
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);
                response = httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            }

            return (T)ob;
        }

        private static void AddHeaders(HttpRequestHeaders httpRequestHeaders, bool isPostOperation = false, string sampleUserContext = "")
        {
            httpRequestHeaders.Accept.Clear();
            //string sampleUserContext = string.Empty;
            httpRequestHeaders.Add("Authorization", sampleUserContext);
            if (isPostOperation)
                httpRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}
