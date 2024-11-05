
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Utilities
{
  public  class ApiManager
    {
        #region summary
        /// <summary>       
        /// Created on : 16-Dec-2019
        /// Created By : 100101
        /// Description: ApiManager
        /// Modify Date:
        /// Modify By  : 
        /// Description:
        /// </summary>

        #endregion

        #region ApiManager
        AppConfigManager appConfigManager;
        public ApiManager()
        {
            appConfigManager = new AppConfigManager();
        }
        #endregion

        #region Methods
        //public Tuple<T, string> InvokePostHttpClient<T, F>(F obj, string url, string token = null)
        //{
        //    string baseUrl = appConfigManager.getBaseUrl;
        //    HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
        //    string contents = JsonConvert.SerializeObject(obj);
        //    var jsonResponse = string.Empty;
        //    Object ob = new object();
        //    using (var httpClient = new HttpClient())
        //    {

        //        httpClient.Timeout = TimeSpan.FromMinutes(60);
        //        if (token != null)
        //        {
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        //        }


        //        response = httpClient.PostAsync(baseUrl + url, new StringContent(contents, Encoding.UTF8, "application/json")).Result;


        //        if (response.IsSuccessStatusCode)
        //        {
        //            jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
        //            ob = JsonConvert.DeserializeObject<T>(jsonResponse);
        //        }
        //    }
        //    return Tuple.Create((T)ob, jsonResponse);
        //}

        //public Tuple<T, string> InvokeXmlHttpClient<T>(string url)
        //{

        //    HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
        //    //< summary >  < Created > Nimitha Francis - 110060 </ Created >
        //    //< summary > New requirement Google API to find distance between hostels (9/8/2019)</ summary >
        //    string googleAPIKeyUrl = appConfigManager.getgoogleAPIKeyUrl;
        //    url = url + googleAPIKeyUrl;
        //    var jsonResponse = string.Empty;
        //    Object ob = new object();
        //    using (var httpClient = new HttpClient())
        //    {

        //        httpClient.Timeout = TimeSpan.FromMinutes(60);


        //        response = httpClient.GetAsync(url).Result;

        //        if (response.IsSuccessStatusCode)
        //        {

        //            string xmlResponse = response.Content.ReadAsStringAsync().Result.ToString();

        //            XmlDocument doc = new XmlDocument();
        //            doc.LoadXml(xmlResponse);
        //            jsonResponse = JsonConvert.SerializeXmlNode(doc);
        //            ob = JsonConvert.DeserializeObject<T>(jsonResponse);
        //        }
        //    }
        //    return Tuple.Create((T)ob, jsonResponse);
        //}
        public T InvokeGetHttpClientWithoutRequest<T>(string url, string token = null)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string baseUrl = appConfigManager.getBaseUrl;
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            using (var httpClient = new HttpClient())
            {
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                cts.CancelAfter(httpClient.Timeout);
                httpClient.DefaultRequestHeaders.Clear();
                response = httpClient.GetAsync(baseUrl + url).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            }

            return (T)ob;
        }
        public Tuple<T, string> InvokeGetHttpClientWithoutRequest<T>(string url)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };

            var jsonResponse = string.Empty;
            Object ob = new object();
            using (var httpClient = new HttpClient())
            {
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                }
            }
            return Tuple.Create((T)ob, jsonResponse);
        }
        public string InvokeGetHttpClientsendsms(string url, string token = null)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string baseUrl = appConfigManager.getBaseUrl;
            var jsonResponse = string.Empty;

            using (var httpClient = new HttpClient())
            {
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                response = httpClient.GetAsync(baseUrl + url).Result;

                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();


            }

            return jsonResponse;
        }
        public Tuple<T, string> InvokePostHttpClient<T, F>(F obj, string url, string authToken = null)
        {
           
            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (authToken != null)
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);
                    }
               
                    ServicePointManager.ServerCertificateValidationCallback = delegate {
                        return true;
                    };


                    cts.CancelAfter(httpClient.Timeout);
                    httpClient.DefaultRequestHeaders.Clear();
                    response = httpClient.PostAsync(baseUrl, new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                        return Tuple.Create((T)ob, jsonResponse);
                    }
                    else
                    {
                        T Obj = Activator.CreateInstance<T>();
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(Obj, jsonResponse);
                    }


                }

            }
            catch (Exception ex)
            {
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);

            }

        }

        public Tuple<string, string, string> InvokeHttpClientAadharApiData(string url, string xml)
        {
            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };

            var jsonResponse = string.Empty;
            var outputResponse = string.Empty;
            Object ob = new object();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var postParams = new Dictionary<string, string>();
                    postParams.Add("aadhar", xml);
                   
                    using (var postContent = new FormUrlEncodedContent(postParams))
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(300); 
                        response = httpClient.PostAsync(baseUrl, postContent).Result;

                        string xmlResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        outputResponse = xmlResponse;
                        var repo = "utf-8";
                        repo = @"<?xml version=""1.0"" encoding=""utf-8""?>";
                        jsonResponse = outputResponse.Replace(repo, "").Trim();
                        //XmlDocument xmlDoc = new XmlDocument();
                        //xmlDoc.LoadXml(jsonResponse);
                        //jsonResponse = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);



                        return Tuple.Create(jsonResponse, xml, outputResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(jsonResponse, xml, outputResponse);
            }

        }
        public Tuple<string, string, string> InvokeHttpClientAadharApiOtp(string url, string xml,string aid)
        {
            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };

            var jsonResponse = string.Empty;
            var outputResponse = string.Empty;
            Object ob = new object();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var postParams = new Dictionary<string, string>();
                    postParams.Add("otp", xml);
                    postParams.Add("aid", aid);

                    using (var postContent = new FormUrlEncodedContent(postParams))
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(300); ;
                        response = httpClient.PostAsync(baseUrl, postContent).Result;

                        string xmlResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        outputResponse = xmlResponse;
                        var repo = "utf-8";
                        repo = @"<?xml version=""1.0"" encoding=""utf-8""?>";
                        jsonResponse = outputResponse.Replace(repo, "").Trim();
                        //XmlDocument xmlDoc = new XmlDocument();
                        //xmlDoc.LoadXml(jsonResponse);
                        //jsonResponse = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);



                        return Tuple.Create(jsonResponse, xml, outputResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(jsonResponse, xml, outputResponse);
            }

        }
        public Tuple<T, string> InvokePostHttpClientPayment<T, F>(F obj, string url, string authToken = null)
        {

            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (authToken != null)
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);
                    }
                    //System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    // Skip validation of SSL/TLS certificate
                    ServicePointManager.ServerCertificateValidationCallback = delegate {
                        return true;
                    };


                    cts.CancelAfter(httpClient.Timeout);
                    httpClient.DefaultRequestHeaders.Clear();
                    response = httpClient.PostAsync(baseUrl, new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                        return Tuple.Create((T)ob, jsonResponse);
                    }
                    else
                    {
                        T Obj = Activator.CreateInstance<T>();
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(Obj, jsonResponse);
                    }


                }

            }
            catch (Exception ex)
            {
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);

            }

        }

        public Tuple<string> InvokePostHttpClient<F>(F obj, string url, string authToken = null)
        {

            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (authToken != null)
                    {
                        httpClient.DefaultRequestHeaders.Add("","");
                    }

                    response = httpClient.PostAsync(baseUrl,null).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                      
                        return Tuple.Create(jsonResponse);
                    }
                    else
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(jsonResponse);
                    }


                }

            }
            catch (Exception ex)
            {
               
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(jsonResponse);

            }

        }

        public T InvokePostHttpClient1<T, F>(F obj, string url,string authToken=null)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
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

            return (T)ob;
        }

        public static T InvokePostHttpClient2<T, F>(F obj, string url, string authToken = null)
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

            return (T)ob;
        }

        #endregion
    }
}
