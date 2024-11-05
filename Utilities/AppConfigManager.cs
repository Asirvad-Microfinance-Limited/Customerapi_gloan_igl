
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utilities
{
   public class AppConfigManager
    {
        #region summary
        /// <summary>       
        /// Created on : 16-Dec-2019
        /// Created By : 100101
        /// Description: AppConfigManager
        /// Modify Date:
        /// Modify By  : 
        /// Description:
        /// </summary>

        #endregion

        #region Declarations
        //-----DB Connection-----//
        public static string DbConnectionString;
        public static string DbPasword;
        public static string baseUrl;
        public static string siteUrl;

        //-----Public API Urls-----//
        public static string accountsApiUrl;
        public static string customerApiUrl;
        public static string generalApiUrl;
        public static string lmsApiUrl;
        public static string losApiUrl;
        
        public static string sharedPath;
        public static string EquifaxapiUrl;
        public static string reportsApiUrl;

        public static string googleAPIKeyUrl;      
        public static string flagGoogleAPISettings;

        //-------MongoDB Connections-------------//
       
        public static string mongoDbName;
        public static string mongoDbPassword;
        public static string mongoUsername;
        public static string mongoAddress;


        #endregion

        #region AppConfigManager
        public AppConfigManager()
        {
            string property = string.Empty;

            try
            {
                //-----DB Connection-----//
                var configurationBuilder = new ConfigurationBuilder();
             
                var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(path, false);
                var root = configurationBuilder.Build();
                DbConnectionString = root.GetSection("ConnectionStrings").GetSection("DbConnection").Value;
                DbPasword = root.GetSection("ConnectionStrings").GetSection("DbPasword").Value;
                baseUrl = root.GetSection("ClientSettings").GetSection("baseUrl").Value;
                siteUrl = root.GetSection("ClientSettings").GetSection("siteUrl").Value;
                EquifaxapiUrl = root.GetSection("EQUIFAXUrl").Value;
                var sharedFolderName = root.GetSection("AppSettingPath").Value;
                var sharedFolderPath = @"../"+ sharedFolderName;               
                sharedPath = System.IO.Path.GetFullPath(sharedFolderPath);

               

                //--------Mongo DB Connection----//          
                //mongoDbName = mongoroot.GetSection("MongoConnectionStrings").GetSection("MongoDbName").Value;
                //mongoDbPassword = mongoroot.GetSection("MongoConnectionStrings").GetSection("MongoDbPasword").Value;
                //mongoUsername = mongoroot.GetSection("MongoConnectionStrings").GetSection("MongoUsername").Value;
                //mongoAddress = mongoroot.GetSection("MongoConnectionStrings").GetSection("MongoAddress").Value;

            }
            catch (Exception exception)
            {
                string stackTrace = exception.StackTrace;
            }

        }

        #endregion

        #region Properties

        public string getConnectionString
        {
            get => DbConnectionString;
        }
        public string getBaseUrl
        {
            get => baseUrl;
        }
        public string getSiteUrl
        {
            get => siteUrl;
        }

        public string getEquifaxapiUrl
        {
            get => EquifaxapiUrl;
        }
        public string getDbPasword
        {
            get => DbPasword;

        }     
        public string getmongoDbName
        {
            get => mongoDbName;

        }
       
        public string getmongoDbPassword
        {
            get => mongoDbPassword;

        }
      
        public string getmongoUsername
        {
            get => mongoUsername;

        }
      
        public string getmongoAddress
        {
            get => mongoAddress;

        }


        public string getaccountsApiUrl
        {
            get => accountsApiUrl;
        }
        public string getcustomerApiUrl
        {
            get => customerApiUrl;
        }
        public string getgeneralApiUrl
        {
            get => generalApiUrl;
        }
        public string getlmsApiUrl
        {
            get => lmsApiUrl;
        }
        public string getlosApiUrl
        {
            get => losApiUrl;
        }
        public string getsharedPath
        {
            get => sharedPath;
        }
        public string getreportsApiUrl
        {
            get => reportsApiUrl;
        }

        #endregion
    }
}
