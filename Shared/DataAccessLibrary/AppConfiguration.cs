using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccessLibrary
{
    public class AppConfiguration
    {
        public readonly string _connectionString = string.Empty;
        public readonly string _serviceUrl = string.Empty;
		public readonly string _baseUrl_payment = string.Empty;

		public readonly string _password = string.Empty;
        public readonly string _isLog = string.Empty;
        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();

            var SharedSettings = Path.Combine(Directory.GetCurrentDirectory(), "SharedSettings.json");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(SharedSettings, false);
            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionStrings").GetSection("DbConnection").Value;
            _serviceUrl = root.GetSection("ServiceUrlSettings").GetSection("baseUrl").Value;
			_baseUrl_payment = root.GetSection("PaymentUrlSettings").GetSection("baseUrl_payment").Value;
			_password = root.GetSection("ConnectionStrings").GetSection("Password").Value;
            _isLog = root.GetSection("isLog").GetSection("status").Value;
        }
        public string ConnectionString
        {
            get => _connectionString;
        }

        public string ServiceUrl
        {
            get => _serviceUrl;
        }

		public string baseUrl_payment
		{
			get => baseUrl_payment;
		}

		public string IsLog
        {
            get => _isLog;
        }
        public string GetPassword {
            get => _password;
        }
    }
}
